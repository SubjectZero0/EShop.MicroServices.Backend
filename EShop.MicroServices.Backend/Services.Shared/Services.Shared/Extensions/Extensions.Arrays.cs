namespace Services.Shared.Extensions
{
	public static partial class Extensions
	{
		public static T[] AsArray<T>(this T element) where T : notnull => [element];
	}
}