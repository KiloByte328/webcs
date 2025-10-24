using web.Models;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValeraTest
{
    public class ValeraTests
    {
        [Fact]
        public void DefaultConstructor_test()
        {
            var ValeraTest = new Valera();
            int Max = 100, Min = 0;
            Assert.Equal(ValeraTest.get_HP(), Max);
            Assert.Equal(ValeraTest.get_MP(), Min);
            Assert.Equal(ValeraTest.get_FT(), Min);
            Assert.Equal(ValeraTest.get_MN(), Min);
            Assert.Equal(ValeraTest.get_CF(), Min);
        }
        [Fact]
        public void Non_default_constructor_test()
        {
            int test = 100;
            var ValeraTest = new Valera(test, test, test, test, test);
            Assert.Equal(ValeraTest.get_HP(), test);
            Assert.Equal(ValeraTest.get_MP(), test);
            Assert.Equal(ValeraTest.get_FT(), test);
            Assert.Equal(ValeraTest.get_MN(), test);
            Assert.NotEqual(ValeraTest.get_CF(), test);
            test = -100;
            var ValeraSecondTest = new Valera(test, test, test, test, test);
            Assert.NotEqual(ValeraSecondTest.get_HP(), test);
            Assert.NotEqual(ValeraSecondTest.get_MP(), test);
            Assert.NotEqual(ValeraSecondTest.get_FT(), test);
            Assert.Equal(ValeraSecondTest.get_MN(), test);
            Assert.NotEqual(ValeraSecondTest.get_CF(), test);
        }
        [Fact]
        public void Limits_test()
        {
            int MaxCom = 100, MinCom = 0;
            var ValeraTest = new Valera();
            ValeraTest.go_to_sleep();
            Assert.Equal(ValeraTest.get_HP(), MaxCom);
            Assert.Equal(ValeraTest.get_MP(), MinCom);
            Assert.Equal(ValeraTest.get_FT(), MinCom);
        }
    }
}