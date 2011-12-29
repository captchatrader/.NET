using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
/**
 * Copyright (C) 2011 by CaptchaTrader http://captchatrader.com/
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * 
 * @author CaptchaTrader
 */
namespace CaptchaTrader
{
    /// <summary>
    /// Create a new CaptchaTrader object.
    /// </summary>
    public class CaptchaTraderAPI
    {
        private string _apiKey;
        private string _username;
        private string _password;
        private string _activeJobId;
        private string _enqueueJobId;
        /// <summary>
        /// Create a new CaptchaTrader instance.
        /// </summary>
        /// <param name="apiKey">The API key of the host application.</param>
        /// <param name="username">The username to run under.</param>
        /// <param name="password">The password of the user to run under.</param>
        public CaptchaTraderAPI(string apiKey, string username, string password)
        {
            _apiKey = apiKey;
            _username = username;
            _password = password;
        }
        /// <summary>
        /// Create a new CaptchaTrader instance.
        /// </summary>
        /// <param name="username">The username to run under.</param>
        /// <param name="password">The password of the user to run under.</param>
        public CaptchaTraderAPI(string username, string password)
        {
            _username = username;
            _password = password;
        }
        /// <summary>
        /// Create a new CaptchaTrader instance.
        /// </summary>
        /// <param name="apiKey">The API key of the host application.</param>
        public CaptchaTraderAPI(string apiKey)
        {
            _apiKey = apiKey;
        }
        /// <summary>
        /// Create a new CaptchaTrader instance.
        /// </summary>
        public CaptchaTraderAPI()
        {
        }
        /// <summary>
        /// The API key of the host application.
        /// </summary>
        public string ApiKey
        {
            get
            {
                return _apiKey;
            }
            set
            {
                _apiKey = value;
            }
        }
        /// <summary>
        /// The username to run under.
        /// </summary>
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }
        /// <summary>
        /// The password of the user. This does not change the user's password.
        /// </summary>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
        /// <summary>
        /// Submit a CAPTCHA URL.
        /// </summary>
        /// <param name="url">The URL of the CAPTCHA.</param>
        /// <returns>The decoded CAPTCHA.</returns>
        public string Submit(Uri url)
        {
            CaptchaTraderRequest request = new CaptchaTraderRequest();
            request.setParameter("value", url.OriginalString);
            return submitJob(request);
        }
        /// <summary>
        /// Submit a CAPTCHA currently saved as a file.
        /// </summary>
        /// <param name="file">The file that the CAPTCHA is saved as.</param>
        /// <returns>The decoded CAPTCHA.</returns>
        public string Submit(FileInfo file)
        {
            CaptchaTraderRequest request = new CaptchaTraderRequest();
            request.setParameter("value", file);
            return submitJob(request);
        }
        /// <summary>
        /// Complete a job submission.
        /// </summary>
        /// <param name="request">The CaptchaTraderRequest of the job.</param>
        /// <returns>The decoded CAPTCHA.</returns>
        private string submitJob(CaptchaTraderRequest request) {
            request.setParameter("username", _username);
            request.setParameter("password", _password);
            request.setParameter("api_key", _apiKey);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(request.post());
            if (doc["error"] != null)
            {
                throw new Exception(doc["error"].InnerText);
            }
            else
            {
                _activeJobId = doc["job"].Attributes["id"].InnerText;
                return doc["job"]["response"].InnerText;
            }
        }
        /// <summary>
        /// Respond to the last sent job.
        /// </summary>
        /// <param name="isCorrect">Whether the job was correct or not.</param>
        public void Respond(bool isCorrect)
        {
            if (_activeJobId == null)
            {
                throw new Exception("INVALID TICKET");
            }
            else
            {
                CaptchaTraderRequest request = new CaptchaTraderRequest(CaptchaTraderRequestType.RESPOND);
                request.setParameter("username", _username);
                request.setParameter("password", _password);
                request.setParameter("ticket", _activeJobId);
                request.setParameter("is_correct", isCorrect ? "1" : "0");

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(request.post());
                if (doc["error"] != null)
                {
                    throw new Exception(doc["error"].InnerText);
                }
                else
                {
                    _activeJobId = null;
                }
            }
        }
        /// <summary>
        /// Get the credits remaining on the current user.
        /// </summary>
        /// <returns>The number of credits remaining.</returns>
        public int GetCredits()
        {
            if (_username == null || _password == null)
            {
                throw new Exception("INVALID USER");
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(new CaptchaTraderRequest(CaptchaTraderRequestType.QUERY_CREDIT, _username, _password).get());
                if (doc["error"] != null)
                {
                    throw new Exception(doc["error"].InnerText);
                }
                else
                {
                    return Int32.Parse(doc["user"]["credits"].InnerText);
                }
            }
        }
        /// <summary>
        /// Get the wait time on the current user. If no user specified, return the wait time for the last user in queue.
        /// </summary>
        /// <returns>The estimated remaining wait time in seconds.</returns>
        public int GetWaitTime()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(new CaptchaTraderRequest(CaptchaTraderRequestType.QUERY_WAIT_TIME, _username, _password).get());
            if (doc["error"] != null)
            {
                throw new Exception(doc["error"].InnerText);
            }
            else
            {
                return Int32.Parse(doc["queue"]["eta"].InnerText);
            }
        }
        /// <summary>
        /// Get the solver queue size.
        /// </summary>
        /// <returns>Two numbers are returned: the number of users ahead of the requesting user and the total queue size.</returns>
        public int[] GetQueueSize()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(new CaptchaTraderRequest(CaptchaTraderRequestType.QUERY_WAIT_TIME, _username, _password).get());
            if (doc["error"] != null)
            {
                throw new Exception(doc["error"].InnerText);
            }
            else
            {
                return new int[] { Int32.Parse(doc["queue"]["position"].InnerText), Int32.Parse(doc["queue"]["length"].InnerText) };
            }
        }
        /// <summary>
        /// Add a user to the job delegation queue.
        /// </summary>
        /// <returns>A data URI of a base64 encoded image.</returns>
        public string Enqueue()
        {
            if (_username == null || _password == null)
            {
                throw new Exception("INVALID USER");
            }
            else if (_enqueueJobId != null)
            {
                throw new Exception("CONNECTION LIMIT");
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(new CaptchaTraderRequest(CaptchaTraderRequestType.ENQUEUE, _username, _password).get());
                if (doc["error"] != null)
                {
                    throw new Exception(doc["error"].InnerText);
                }
                else
                {
                    _enqueueJobId = doc["job"].Attributes["id"].InnerText;
                    return doc["job"]["challenge"].InnerText;
                }
            }
        }
        /// <summary>
        /// Remove the user from all pending jobs and the job queue.
        /// </summary>
        public void Dequeue()
        {
            if (_username == null || _password == null)
            {
                throw new Exception("INVALID USER");
            }
            else
            {
                CaptchaTraderRequest request = new CaptchaTraderRequest(CaptchaTraderRequestType.DEQUEUE);
                request.setParameter("username", _username);
                request.setParameter("password", _password);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(request.post());
                if (doc["error"] != null)
                {
                    throw new Exception(doc["error"].InnerText);
                }
                else
                {
                    _enqueueJobId = null;
                }
            }
        }
        /// <summary>
        /// Provide an answer to a job.
        /// </summary>
        /// <param name="value">The answer to submit.</param>
        public void Answer(string value)
        {
            if (_username == null || _password == null)
            {
                throw new Exception("INVALID USER");
            }
            else if (_enqueueJobId == null)
            {
                throw new Exception("INVALID TICKET");
            }
            else
            {
                CaptchaTraderRequest request = new CaptchaTraderRequest(CaptchaTraderRequestType.ANSWER);
                request.setParameter("username", _username);
                request.setParameter("password", _password);
                request.setParameter("ticket", _enqueueJobId);
                request.setParameter("value", value);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(request.post());
                if (doc["error"] != null)
                {
                    throw new Exception(doc["error"].InnerText);
                }
                else
                {
                    _enqueueJobId = null;
                }
            }
        }
    }
}
