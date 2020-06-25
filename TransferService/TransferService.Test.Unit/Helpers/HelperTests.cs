﻿using TransferService.Helper.Extensions;
using TransferService.Helper.Image;
using Xunit;

namespace TransferService.Test.Unit.Helpers
{
    public class HelperTests
    {
        [Fact]
        public void GenerateSlugValid()
        {
            var phrase = "Harry Potter and the Philosopher's Stone";

            var actual = phrase.GenerateSlug();
            var expected = "harry-potter-and-the-philosophers-stone";

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void GenerateSlugInvalid()
        {
            var phrase = "Harry Potter and the Philosopher's Stone";

            var actual = phrase.GenerateSlug();
            var expected = "Harry-potter-and-thE-philosophers-stone";

            Assert.NotEqual(actual, expected);
        }

        [Fact]
        public void FormatImageValid()
        {

            var imageName = "image-name.png";

            var title = "Harry Potter and the Philosopher's Stone";

            var expected = "harry-potter-and-the-philosophers-stone.png";

            var actual = ImageHelper.FormatImageName(imageName, title.GenerateSlug());

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void ImageUrlValid()
        {
            var expected = @"http://dev.sharebook.com.br/Images/Books/image.jpg";
            var actual = ImageHelper.GenerateImageUrl("image.jpg", "wwwroot/Images/Books", "http://dev.sharebook.com.br");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IncrementalIsValidCopy1()
        {
            var phrase = "Harry-potter-and-thE-philosophers-stone";
            var expected = "Harry-potter-and-thE-philosophers-stone_copy1";
            var actual = phrase.AddIncremental();

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void IncrementalIsValidCopy2()
        {
            var phrase = "Harry-potter-and-thE-philosophers-stone_copy1";
            var expected = "Harry-potter-and-thE-philosophers-stone_copy2";
            var actual = phrase.AddIncremental();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IncrementalIsValidCopy11002()
        {
            var phrase = "Harry-potter-and-thE-philosophers-stone_copy11001";
            var expected = "Harry-potter-and-thE-philosophers-stone_copy11002";
            var actual = phrase.AddIncremental();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IncrementalIsValidCopy100()
        {
            var phrase = "Harry-potter-and-thE-philosophers-stone_copy99";
            var expected = "Harry-potter-and-thE-philosophers-stone_copy100";
            var actual = phrase.AddIncremental();

            Assert.Equal(expected, actual);
        }
    }
}
