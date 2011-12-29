using System;
using System.Collections.Generic;
using System.Text;
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
    class CaptchaTraderRequestType
    {
        public static readonly CaptchaTraderRequestType SUBMIT = new CaptchaTraderRequestType("http://api.captchatrader.com/submit", true);
        public static readonly CaptchaTraderRequestType RESPOND = new CaptchaTraderRequestType("http://api.captchatrader.com/respond", true);
        public static readonly CaptchaTraderRequestType QUERY_CREDIT = new CaptchaTraderRequestType("http://api.captchatrader.com/get_credits", false);
        public static readonly CaptchaTraderRequestType QUERY_WAIT_TIME = new CaptchaTraderRequestType("http://api.captchatrader.com/get_wait_time", false);
        public static readonly CaptchaTraderRequestType ENQUEUE = new CaptchaTraderRequestType("http://api.captchatrader.com/enqueue", false);
	    public static readonly CaptchaTraderRequestType ANSWER = new CaptchaTraderRequestType("http://api.captchatrader.com/answer", true);
        public static readonly CaptchaTraderRequestType DEQUEUE = new CaptchaTraderRequestType("http://api.captchatrader.com/dequeue", true);
        public static IEnumerable<CaptchaTraderRequestType> Values
        {
            get
            {
                yield return SUBMIT;
                yield return RESPOND;
                yield return QUERY_CREDIT;
                yield return QUERY_WAIT_TIME;
                yield return ENQUEUE;
                yield return ANSWER;
                yield return DEQUEUE;
            }
        }
        private string _url;
        private bool _doPost;
        public CaptchaTraderRequestType(string url, bool doPost)
        {
            _url = url;
            _doPost = doPost;
        }
        public string url
        {
            get
            {
                return _url;
            }
        }
        public bool doPost
        {
            get
            {
                return _doPost;
            }
        }

    }
}
