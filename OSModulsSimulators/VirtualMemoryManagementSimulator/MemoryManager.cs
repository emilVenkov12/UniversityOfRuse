namespace VirtualMemoryManagementSimulator
{
    using System;
    using System.Collections.Generic;

    class MemoryManager
    {
        private int realMemorySize;

        private Frame[] realMemory;

        public MemoryManager(int memorySize)
        {
            this.realMemorySize = memorySize;
            realMemory = new Frame[this.realMemorySize];
        }
        public void LoadProcess(string processName)
        {
            Frame frameToLoad = new Frame() { ProcessName = processName, AccessesTime = DateTime.Now };

            bool isFreeSlotFound = false;
            for (int i = 0; i < this.realMemorySize; i++)
            {
                if (this.realMemory[i] == null)
                {
                    this.realMemory[i] = frameToLoad;
                    isFreeSlotFound = true;
                    break;
                }
            }

            if (!isFreeSlotFound)
            {
                int frameToReplaceIndex = GetFrameToRepalceIndex();
                this.realMemory[frameToReplaceIndex] = frameToLoad;
            }
        }

        private int GetFrameToRepalceIndex()
        {
            int replaceFrameIndex = 0;
            for (int i = 0; i < this.realMemorySize; i++)
            {
                if (this.realMemory[replaceFrameIndex].AccessesTime >= this.realMemory[i].AccessesTime)
                {
                    replaceFrameIndex = i;
                }
            }

            return replaceFrameIndex;
        }

        public void AccessFrame(int frameIndex)
        {
            if (frameIndex < 0 || frameIndex > this.realMemorySize)
            {
                throw new ArgumentException("frame index should be bigger"+
                    " than -1 and less than memory size !");
            }

            this.realMemory[frameIndex].AccessesTime = DateTime.Now;
        }

        public override string ToString()
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder();
            for (int i = 0; i < this.realMemorySize; i++)
            {
                if (this.realMemory[i] != null)
                {
                    output.AppendFormat("Frame №: {0}, procesesID : {1}, last accessed at {2}", 
                        i, this.realMemory[i].ProcessName, this.realMemory[i].AccessesTime);
                    output.AppendLine(Environment.NewLine);
                }
            }
            if (output.Length == 0)
            {
                output.AppendLine("The memory is empty.");
            }

            return output.ToString();
        }
    }
}
