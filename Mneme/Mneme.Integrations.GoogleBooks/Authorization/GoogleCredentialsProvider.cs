﻿namespace Mneme.Integrations.GoogleBooks.Authorization;

public class GoogleCredentialsProvider
{
	private readonly string filePath = @"Google/googleCreds.json";
	public Func<FileStream> GetFileStream() => () => new FileStream(filePath, FileMode.Open, FileAccess.Read);
}
