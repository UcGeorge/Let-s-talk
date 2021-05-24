using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{

    public class ChatFile
    {
        private string fileName;
        private byte[] content;
        private string sender;

        public ChatFile(string fileName, byte[] content, string sender)
        {
            this.content = content;
            this.sender = sender;
            this.fileName = fileName;
        }

        public (string, byte[]) GetFile { get => (fileName, content); }
        public string Sender { get => sender; }
    }
}
