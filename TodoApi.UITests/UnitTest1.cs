using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TodoApi.UITests;

public class LoginTest: IDisposable
{
    private static string _urlUnderTesting = "https://localhost:7123/";
    private readonly IWebDriver _driver;

    public LoginTest()
    {
        var chromeOptions = new ChromeOptions();

        chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
        _driver = new ChromeDriver(chromeOptions);
    }

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    [Fact]
    void LoginScreenVisible()
    {
        _driver.Navigate()
            .GoToUrl(_urlUnderTesting);

        Assert.Equal("Todo App", _driver.Title);
    }
    
    [Theory]
    [InlineData("ldangelo", "Harley1234!", false)]
    [InlineData("foo", "bar!", true)]
    public void LoginTests(string username, string password,bool shouldFail)
    {
        _driver.Navigate()
            .GoToUrl(_urlUnderTesting);

        // 
        // Intput username and password and submit
        _driver.FindElement(By.Id("username"))
            .SendKeys(username);

        _driver.FindElement(By.Id("password"))
            .SendKeys(password);

        _driver.FindElement(By.ClassName("btn-primary"))
            .Click();

        if (shouldFail)
        {
            //
            // check for validation errors
            var validation = _driver.FindElement(By.ClassName("validation-message"));
            Assert.NotEmpty(validation.Text); 
        }
        else
        {
            Assert.Equal("Todo App", _driver.Title);
        }
    }
}
