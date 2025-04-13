using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LMS
{
    public static class HtmlMAIL
    {
        public static string htmlBody(string name, string verificationCode)
        {
            DateTime expirationTime = DateTime.Now.AddMinutes(2);
            string dateTimeNow = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            string expirationTimeStr = expirationTime.ToString("yyyy/MM/dd HH:mm");

            // Create HTML content
            string htmlContent = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f5f5f5;
                            margin: 0;
                            padding: 0;
                            max-width: 600px;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 30px auto;
                            background: #ffffff;
                            border-radius: 10px;
                            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                            padding: 20px;
                        }}
                        .header {{
                            text-align: center;
                            padding: 20px;
                            background-color: #D9C2A3;
                            border-top-left-radius: 10px;
                            border-top-right-radius: 10px;
                        }}
                        .header img {{
                            width: 150px;
                            margin-bottom: 10px;
                        }}
                        .header h1 {{
                            color: #4A4A4A;
                            font-size: 22px;
                            margin: 0;
                        }}
                        .content {{
                            padding: 20px;
                            text-align: center;
                        }}
                        .content p {{
                            font-size: 16px;
                            color: #333;
                            margin-bottom: 15px;
                        }}
                        .code-box {{
                            display: inline-block;
                            font-size: 24px;
                            font-weight: bold;
                            color: #333;
                            background-color: #F8E1A6;
                            padding: 15px 25px;
                            border-radius: 5px;
                            margin-top: 20px;
                            letter-spacing: 3px;
                        }}
                        .footer {{
                            text-align: center;
                            padding: 15px;
                            font-size: 12px;
                            color: #888;
                            border-top: 1px solid #ddd;
                        }}
                        .footer a {{
                            color: #D9C2A3;
                            text-decoration: none;
                            font-weight: bold;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <img src='https://i.ibb.co/4nZQx708/Full-Logo-Transparent.png' alt='RoadRunner Logo'/>
                            <h1>Verify Your RoadRunner Account</h1>
                        </div>
                        <div class='content'>
                            <p>Dear {name},</p>
                            <p>Thank you for signing up for RoadRunner Delivery! Please enter the verification code below to complete your registration:</p>
                            <p>Sent on: {dateTimeNow}</p>
                            <div class='code-box'>{verificationCode}</div>
                            <p>This code will expire on: {expirationTimeStr}</p>
                            <p>If you did not request this, please ignore this email.</p>
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024 RoadRunner Delivery. All rights reserved.</p>
                            <p>Need help? <a href='https://roadrunner.com/support'>Contact Support</a></p>
                        </div>
                    </div>
                </body>
                </html>";
            return htmlContent;
        }
    }
}
