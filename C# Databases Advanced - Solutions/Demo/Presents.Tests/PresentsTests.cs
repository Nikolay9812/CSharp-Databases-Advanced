namespace Presents.Tests
{
    using Christmas;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class PresentsTests
    {
        [Test]
        public void ConstructorInitCorrectly()
        {
            Present present = new Present("Blue", 10);

            Assert.AreEqual("Blue", present.Color);
            Assert.AreEqual(10, present.Capacity);
        }

        [Test]
        public void CreateShoudlThrowEception()
        {
            Assert.Throws<ArgumentNullException>(() new Present
        }
    }
}
