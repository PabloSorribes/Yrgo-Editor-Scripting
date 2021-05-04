namespace Paalo.UnityMiscTools
{
	public class CoroutineCancellationToken
	{
		private int numInit;
		private int numFinished;
		private bool cancel;

		public bool IsFinished => numFinished >= numInit && numInit > 0;
		public int FinishedTokens => numFinished;
		public bool IsCanceled => cancel;

		public void CoroutineStart()
		{
			numInit++;
		}

		public void CoroutineFinish()
		{
			numFinished++;
		}

		public void Cancel()
		{
			cancel = true;
		}
	}
}