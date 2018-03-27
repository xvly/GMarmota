namespace GStd.Editor
{
    using System;
    using UnityEditor;
    public sealed class ProgressIndicator : IDisposable
    {
        private string title;

		private int total = 1;

		private int count;

		private double progress = 0.03;

		private double time;

		public ProgressIndicator(string title, int total)
        {
            this.title = title;
            this.total = total;
        }

        public void Next(int count = 1)
        {
            this.count += count;
        }

        public bool Show(string message)
        {
            if (this.total <= this.count)
			{
                return false;
            }
            return EditorUtility.DisplayCancelableProgressBar(this.title, message, (float)this.count / (float)this.total);
        }

        public bool Show(string format, params object[] args)
        {
            return this.Show(string.Format(format, args));
        }

        public void Dispose()
        {
            EditorUtility.ClearProgressBar();
        }
    }
}
