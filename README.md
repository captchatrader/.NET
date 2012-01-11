CaptchaTrader .NET API
======================

This library allows interaction with http://captchatrader.com/ through all .NET CLI languages.

Usage
-----

Compile the C# code with Visual Studio or download the provided .dll file.  Include this library either in Visual Studio or at compile-time.

Public API
----------

The following documentation is also attached to the dll library.

### Constructor

Create a new CaptchaTrader instance.

	public CaptchaTraderAPI(string apiKey, string username, string password)
	public CaptchaTraderAPI(string username, string password)
	public CaptchaTraderAPI(string apiKey)
	public CaptchaTraderAPI()

### Submit

Submit a CAPTCHA URL.

	public string Submit(Uri url)
	public string Submit(FileInfo file)
	
### Respond

Respond to the last sent job.

	public void Respond(bool isCorrect)

### GetCredits

Get the credits remaining on the current user

	public int GetCredits()

### GetWaitTime

Get the wait time on the current user.  If no user specified, return the wait time for the last user in queue.

	public int GetWaitTime()

### GetQueueSize

Get the solver queue size. Two numbers are returned: the number of users ahead of the requesting user and the total queue size.

	public int[] GetQueueSize()

### Enqueue

Add a user to the job delegation queue.

	public string Enqueue()

### Dequeue

Remove the user from all pending jobs and the job queue.

	public void Dequeue()

### Answer

Provide an answer to a job.

	public void Answer(string value)

Synchronous Submit Example
--------------------------

	CaptchaTraderAPI ct = new CaptchaTraderAPI(<apiKey>, <username>, <password>);
	Uri url = new Uri("http://www.google.com/recaptcha/api/image?c=03AHJ_VuuH-DBRSxMQgwIJM4L5B5-CmEDLCigmIPZcc50vRJVSXRIp0dDZKRskWTXgiM7m0T2nus0PH4gFWC74QPWjX9W9dzpN-qpRWQJ3GO7v4nF9oDCvI9TtfISCFeIcwzMJbh4aqfOq1_rhWjJ0Pmpbu-Uy1-Yj7A");
	string solution = ct.Submit(url);
	ct.Respond(true);