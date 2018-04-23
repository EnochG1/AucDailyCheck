using System;
using System.Linq;
using System.Configuration;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace AuctionDailyCheck
{
	class RunSelenium
	{
		static void Main(string[] args)
		{
			string id = Convert.ToString(ConfigurationManager.AppSettings["ID"]);
			string pw = Convert.ToString(ConfigurationManager.AppSettings["PW"]);

			Console.WriteLine("ID : " + id);

			if (string.IsNullOrEmpty(id))
			{
				Console.WriteLine("ID value is Empty. Check AuctionDailyCheck.exe.config");
				Console.ReadLine();
				Environment.Exit(0);
			}

			IWebDriver driver = new ChromeDriver();


			try
			{
				driver.Url = "https://memberssl.auction.co.kr/Authenticate";

				IWebElement element;

				WebDriverWait waitForElement = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
				waitForElement.Until(ExpectedConditions.ElementIsVisible(By.Id("id")));

				element = driver.FindElement(By.Id("id"));
				element.SendKeys(id);
				element = driver.FindElement(By.Id("password"));
				element.SendKeys(pw);
				element = driver.FindElement(By.CssSelector("input[type=submit][value='로그인']"));
				element.Click();

				driver.Navigate().GoToUrl("http://eventv2.auction.co.kr/event3/Regular/EverydayPoint/IfrmMainContents.aspx");

				waitForElement = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
				waitForElement.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".swiper-wrapper")));

				//ReadOnlyCollection<IWebElement> pointEls = driver.FindElements(By.CssSelector(".smile_point_list .btn_point"));
				IJavaScriptExecutor js = driver as IJavaScriptExecutor;

				js.ExecuteScript("$('.smile_point_list .btn_point').each(function () { $(this).trigger('click'); });");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}
		}
	}
}
