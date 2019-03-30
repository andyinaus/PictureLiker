using PictureLiker.DAL;
using Xunit;

namespace PictureLiker.Tests.DAL
{
    public class PreferenceTests
    {
        [Fact]
        public void SetIsLiked_WhenIsLikedIsTrue_ShouldSetCorrectly()
        {
            var preference = new Preference(0, 1);

            preference.SetIsLiked(true);

            Assert.True(preference.IsLiked);
        }

        [Fact]
        public void SetIsLiked_WhenIsLikedIsFalse_ShouldSetCorrectly()
        {
            var preference = new Preference(0, 1);

            preference.SetIsLiked(false);

            Assert.False(preference.IsLiked);
        }
    }
}