using Dsmviz.Viewer.Utils;

namespace Dsmviz.Test.Util
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void GivenNotMatchingReplaceText_WhenReplaceIgnoreCaseIsCalledWith_ThenStringIsNotModified()
        {
            Assert.AreEqual("abc", "abc".ReplaceIgnoreCase("d", "e"));
        }

        [TestMethod]
        public void GivenMatchingLowerCaseReplaceText_WhenReplaceIgnoreCaseIsCalled_ThenStringIsModified()
        {
            Assert.AreEqual("xyzdef", "abcdef".ReplaceIgnoreCase("abc", "xyz"));
        }

        [TestMethod]
        public void GivenMatchingUpperCaseReplaceText_WhenReplaceIgnoreCaseIsCalled_ThemStringIsModified()
        {
            Assert.AreEqual("xyzdef", "ABCdef".ReplaceIgnoreCase("abc", "xyz"));
        }

        [TestMethod]
        public void GivenMatchingNonAlphabeticReplaceText_WhenReplaceIgnoreCaseIsCalled_ThenStringIsModified()
        {
            Assert.AreEqual("xyzdef", "A*BCdef".ReplaceIgnoreCase("a*bc", "xyz"));
            Assert.AreEqual("x*yzdef", "abcdef".ReplaceIgnoreCase("abc", "x*yz"));
            Assert.AreEqual("$def", "abcdef".ReplaceIgnoreCase("abc", "$"));
        }
    }
}
