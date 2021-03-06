﻿using NUnit.Framework;
using ServiceStack.Text;
using System.Collections.Generic;

namespace NppGist.Tests
{
    [TestFixture]
    public class GitHubAPITests
    {
        [Test]
        public void GetUser()
        {
            var response = Utils.SendRequest("https://api.github.com/users/KvanTTT", out var responseHeaders);
            var user = JsonSerializer.DeserializeFromString<User>(response);
        }

        [Test]
        public void GetGists()
        {
            var gists = Utils.SendJsonRequest<List<Gist>>("https://api.github.com/gists");
            Assert.Greater(gists.Count, 0);
        }
    }
}
