using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Thong.Net
{
    public sealed class Message
    {
        
        private MemoryStream Stream { get; }
        public BinaryReader Reader { get; }
        public BinaryWriter Writer { get; }
       
        public byte Command { get; }
        
        public Message(byte command)
        {
            Command = command;
            Stream = new MemoryStream();
            Writer = new BinaryWriter(Stream);
        }
        public Message(byte command, byte[] buffer)
        {
            Command=command;
            Stream = new MemoryStream(buffer);
            Reader = new BinaryReader(Stream);
        }
       
        public byte[] Data
        {
            get
            {
                if (Stream != null)
                {
                    return Stream.ToArray();
                }
                return new byte[0];
            }
        }
    }
}
