using System;
using System.Text;
using System.Collections;
using System.IO.Ports;
using UnityEngine;

//NOTE: Make sure you set the API Compatibility Level to NET 4.x or this will not work

public class SerialController : MonoBehaviour
{
    [SerializeField] protected string port;                   // i.e. COM4
    [SerializeField] protected int ID;                        // ID of Arduino (Based on excel sheet)
    [SerializeField] protected int baudrate = 9600;           // Make sure this matches the baud rate of the Arduino
    [SerializeField] protected bool debugPrint = false;       // true to print messages to console
    [SerializeField] protected bool autoConnect = true;      // true to connect on instantiation
    [SerializeField] protected bool autoReconnect = true;    // will close and attempt to re-open port after disconnect
    [SerializeField, Range(1, BUFSIZE)]
    protected int maxMessageLength = 50;                      // maximum length of expected messages

    protected const int BUFSIZE = 2048;   // buffer for storing all incoming bytes and parsing them
    protected const byte CR = 0xD;        // carriage return byte
    protected const byte LF = 0xA;        // line feed (newline) byte - write Arduino sketch to use Println or use LF as line terminator

    protected byte[] buf = new byte[BUFSIZE];                 // buffer for writing incoming serial
    protected byte[] messageBuilder;                          // buffer for storing incoming serial during parsing
    protected int idx = 0;                                    // index in messageBuilder while parsing

    protected bool connected = false;
    protected SerialPort stream;
    protected IEnumerator read;

    public static SerialController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // init messageBuilder to size of longest message expected
        messageBuilder = new byte[maxMessageLength];

        if (autoConnect)
        {
            Begin();
        }
    }

    public bool Connected
    {
        get
        {
            return connected;
        }
    }

    public virtual bool Begin()
    {
        bool success = true;
        if (!connected)
        {
            try
            {
                stream = new SerialPort(@"\\.\" + port, baudrate);
                success = TryConnect();
            }
            catch (System.Exception e)
            {
                print(Prepend(e.Message));
                success = false;
            }
            if (success)
            {
                read = ReadSerial();
                StartCoroutine(read);
            }
        }
        else
        {
            print(Prepend("Already connected"));
        }
        connected = success;
        return success;
    }

    protected virtual bool TryConnect()
    {
        print(Prepend("Attempting to open port."));
        try
        {
            stream = new SerialPort(@"\\.\" + port, baudrate);
            stream.ReadTimeout = 1;
            stream.Open();
            print(Prepend("Open."));
        }
        catch (Exception e)
        {
            print(Prepend(e.Message));
            return false;
        }
        return true;
    }

    protected virtual IEnumerator ReadSerial()
    {
        while (true)
        {
            if (stream.IsOpen)
            {
                try
                {
                    // read up to maxLen bytes from the Serial buffer
                    int readCount = stream.Read(buf, 0, BUFSIZE);
                    // send buffer on for parsing out the messages
                    ParseBuffer(buf, readCount);
                }
                catch (TimeoutException)
                {
                }
                catch (System.IO.IOException e)
                {
                    // Reader probably unplugged.  Close so that IsOpen() will read false
                    print(Prepend(e.Message));
                    print(Prepend("Closing connection."));
                    stream.Close();
                    connected = false;
                }
                catch (Exception e)
                {
                    print(Prepend(e.ToString()));
                }
            }
            // if port was closed after disconnect and autoReconnect
            else if (autoReconnect)
            {
                do
                {
                    connected = TryConnect();
                    yield return new WaitForSeconds(1f);
                } while (!stream.IsOpen);
            }

            yield return null;
        }
    }

    protected void ParseBuffer(byte[] buf, int len)
    {
        // parse up to len bytes (should be the number read into buf)
        for (int i = 0; i < len; i++)
        {
            byte inByte = buf[i];
            switch (inByte)
            {
                // ignore carriage return
                case CR:
                    break;
                // message end is line-feed, reset idx for next message
                case LF:
                    HandleMessage(messageBuilder, idx);
                    idx = 0;
                    break;
                // anything else is part of message, write it to next position
                default:
                    if (idx > maxMessageLength)
                    {
                        print(Prepend("Message length error.  Too long."));
                    }
                    else
                    {
                        messageBuilder[idx++] = inByte;
                    }
                    break;
            }
        }
    }

    protected void HandleMessage(byte[] m, int len)
    {
        string s = Encoding.UTF8.GetString(m).Substring(0, len);
        if (debugPrint)
        {
            print(Prepend("received: " + Encoding.UTF8.GetString(m).Substring(0, len)));
        }

        // Message_Controller.instance.DelegateMessage(s);
    }

    // used to identify stamp messages to identify
    // the source as this Arduino/port
    protected virtual string Prepend(string message)
    {
        return "Arduino " + port + " " + ID + ": " + message;
    }

    // send message to Arduino
    public void MessageSend(string messageToSend)
    {
        WriteLine(messageToSend);
    }

    // Send a string to the Arduino, terminated with 
    // SerialPort.NewLine (defaults to line feed)
    public void WriteLine(string m)
    {
        try
        {
            stream.WriteLine(m);
        }
        catch (Exception e)
        {
            print(Prepend(e.Message));
        }
    }
}
