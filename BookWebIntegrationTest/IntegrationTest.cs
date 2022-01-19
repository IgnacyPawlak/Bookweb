using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using System.Collections.Generic;

namespace BookWebIntegrationTest
{
    public class Tests
    {

        public string homeUrl = "https://localhost:44317";
        //[Test]
        //public void GeneralTest()
        //{
        //    //browser driver
        //    IWebDriver webDriver = new ChromeDriver();
        //    //navigate to site
        //    webDriver.Navigate().GoToUrl("https://localhost:44317");
        //    //actions for unlogged
        //    ShowBookListTest(webDriver, "unlogged");
        //    SearchByTitleTest(webDriver);

        //    //log in as user
        //    LoginTest(webDriver, "user@example.com", "User123!", "welcome-back-user");
        //    //tests for user
        //    ShowBookListTest(webDriver, "user");
        //    SearchByTitleTest(webDriver);
        //    AddToFavoriteTest(webDriver, "user");

        //    LogoutTest(webDriver);
        //}

            [Test]
        public void AdminTest()
        {
            //browser driver
            IWebDriver webDriver = new ChromeDriver();
            //navigate to site
            webDriver.Navigate().GoToUrl(homeUrl);
            //log in as admin
            LoginTest(webDriver, "admin@example.com", "Test123!", "welcome-back-admin");
            ShowBookListTest(webDriver, "admin");
            SearchByTitleTest(webDriver);
            AddToFavoriteTest(webDriver);

            LogoutTest(webDriver);
            webDriver.Close();
        }

        [Test]
        public void UserTest()
        {
            //browser driver
            IWebDriver webDriver = new ChromeDriver();
            //navigate to site
            webDriver.Navigate().GoToUrl(homeUrl);
            //log in as user
            LoginTest(webDriver, "user@example.com", "Test123!", "welcome-back-user");
            ShowBookListTest(webDriver, "user");
            SearchByTitleTest(webDriver);
            AddToFavoriteTest(webDriver);

            LogoutTest(webDriver);
            webDriver.Close();
        }
               
        [Test]
        public void UnloggedTest()
        {
            //browser driver
            IWebDriver webDriver = new ChromeDriver();
            //navigate to site
            webDriver.Navigate().GoToUrl(homeUrl);
            ShowBookListTest(webDriver, "unlogged");
            SearchByTitleTest(webDriver);
            webDriver.Close();
        }

        public void LoginTest(IWebDriver webDriver, string email, string password, string welcomeFieldId)
        {
            //log in button click
            IWebElement loginBtn = webDriver.FindElement(By.LinkText("Log in"));
            loginBtn.Click();

            //login form display check
            var loginBox = webDriver.FindElement(By.Id("login-box"));
            Assert.That(loginBox.Displayed, Is.True);

            //login form fill and submit
            var emailInputLogin = webDriver.FindElement(By.Name("Input.Email"));
            var passwordInputLogin = webDriver.FindElement(By.Name("Input.Password"));
            var loginSubmitBtn = webDriver.FindElement(By.Id("login-submit-btn"));
            emailInputLogin.SendKeys(email);
            passwordInputLogin.SendKeys(password);
            loginSubmitBtn.Click();

            //welcome page display check
            var welcomeBack = webDriver.FindElement(By.Id(welcomeFieldId));
            Assert.That(welcomeBack.Displayed, Is.True);
        }

        public void LogoutTest(IWebDriver webDriver)
        {
            //logout button click
            IWebElement logoutBtn = webDriver.FindElement(By.Id("logout"));
            logoutBtn.Click();

            //log in sign up buttons display check
            IWebElement loginSignupBox = webDriver.FindElement(By.Id("login-signup"));
            Assert.That(loginSignupBox.Displayed, Is.True);
        }
        public void ShowBookListTest(IWebDriver webDriver, string role)
        {
            //click Find a book button
            IWebElement findABookBtn = webDriver.FindElement(By.Id("find-a-book-btn"));
            findABookBtn.Click();

            //check if book list is displayed
            IWebElement bookList = webDriver.FindElement(By.Id("book-list"));
            Assert.That(bookList.Displayed, Is.True);

            //string length 80 => empty list of elements
            if (role == "admin")
            {
                //add to favorite button display check (available for user and admin)
                IWebElement addToFavoriteBtn = webDriver.FindElement(By.LinkText("Add to Favorite"));
                Assert.That(addToFavoriteBtn.Displayed, Is.True);

                //remove button display check (available only for admin)               
                IWebElement removeBtn = webDriver.FindElement(By.ClassName("remove-btn"));
                Assert.That(removeBtn.Displayed, Is.True);

                //add a  book test
                AddABookTest(webDriver, "admin");
            }

            if (role == "user")
            {
                //add to favorite button display check (available for user and admin)
                IWebElement addToFavoriteBtn = webDriver.FindElement(By.LinkText("Add to Favorite"));
                Assert.That(addToFavoriteBtn.Displayed, Is.True);

                AddABookTest(webDriver, "user");
            }

            if (role == "unlogged")
            {
                
            }

        }
        public void AddABookTest(IWebDriver webDriver, string role)
        {
            //ensure current view is book list (Find a book)
            var findABookBtn = webDriver.FindElement(By.Id("find-a-book-btn"));
            findABookBtn.Click();


            int previousBookAmount = 0;
            int currentBookAmount = 0;

            if (role == "admin")
            {
                //get amount of books before adding a new one
                previousBookAmount = webDriver.FindElements(By.ClassName("book-display-line")).Count;
            }

            //add a book button
            IWebElement addBookBtn = webDriver.FindElement(By.Id("add-book"));
            addBookBtn.Click();

            //fill in add a book form
            IWebElement title = webDriver.FindElement(By.Name("productTitle"));
            IWebElement author = webDriver.FindElement(By.Name("productAuthor"));
            IWebElement description = webDriver.FindElement(By.Name("productDescription"));
            IWebElement addSubmitBtn = webDriver.FindElement(By.Name("addSubmitBtn"));
            Assert.That(title.Displayed, Is.True);
            title.SendKeys("title");
            author.SendKeys("author");
            description.SendKeys("description");
            addSubmitBtn.Click();

            

            if (role == "admin")
            {
                //get current amount of books to new variable
                currentBookAmount = webDriver.FindElements(By.ClassName("book-display-line")).Count;
                //check if a book got added
                Assert.Greater(currentBookAmount, previousBookAmount);
            }
            if(role == "user" || role == "unlogged")
            {
                //check if book list is displayed - book is visible only after admin accepts it
                IWebElement bookList = webDriver.FindElement(By.Id("book-list"));
                Assert.That(bookList.Displayed, Is.True);
            }
        }
        public void SearchByTitleTest(IWebDriver webDriver)
        {
            //ensure current view is book list (Find a book)
            var findABookBtn = webDriver.FindElement(By.Id("find-a-book-btn"));
            findABookBtn.Click();

            //find search box and search submit button
            IWebElement searchBox = webDriver.FindElement(By.Id("search-title-input"));
            IWebElement searchBtn = webDriver.FindElement(By.Name("searchSubmitBtn"));

            //search box fill and submit
            searchBox.SendKeys("title"); //button will forward to search by title page if input isn't empty or blank, regardless of the title being on the list
            searchBtn.Click();

            //check see all books button display at search by title page
            IWebElement seeAllBooksBtn = webDriver.FindElement(By.Name("seeAllBooksBtn"));
            Assert.That(seeAllBooksBtn.Displayed, Is.True);
        }
        public void AddToFavoriteTest(IWebDriver webDriver)
        {
            //ensure current view is: My favorite books
            IWebElement favoriteBooksBtn = webDriver.FindElement(By.LinkText("My favorite books"));
            favoriteBooksBtn.Click();           

                //get amount of  favorite books before adding a new one
                int previousBookAmount = webDriver.FindElements(By.ClassName("book-display-line")).Count;

            //go to: Find a book
            var findABookBtn = webDriver.FindElement(By.LinkText("Find a book"));
            findABookBtn.Click();

                //add book to favorite
                IWebElement addToFavoriteBtn = webDriver.FindElement(By.LinkText("Add to Favorite"));
                addToFavoriteBtn.Click();

            //go back to: My favorite books
            IWebElement favoriteBooksBtn1 = webDriver.FindElement(By.LinkText("My favorite books"));
            favoriteBooksBtn1.Click();

                //get current amount of favorite books to new variable
                int currentBookAmount = webDriver.FindElements(By.ClassName("book-display-line")).Count;

                //check if a book got added
                Assert.Greater(currentBookAmount, previousBookAmount);

            //remove added book before next test
            if (webDriver.FindElements(By.LinkText("Remove from favorite list")).Count > 0)
            {
                for (int i =0; i < webDriver.FindElements(By.LinkText("Remove from favorite list")).Count; i++)
                {
                    IWebElement removeBtn = webDriver.FindElement(By.LinkText("Remove from favorite list"));
                    removeBtn.Click();
                }

            }

        }
    }
}