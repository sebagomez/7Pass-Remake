﻿using System;
using System.Linq;
using System.Xml.Linq;
using SevenPass.Entry.ViewModels;
using Xunit;

namespace SevenPass.Tests.ViewModels.Entry
{
    public class EntryFieldsViewModelTests
        : EntrySubViewTestsBase<EntryFieldsViewModel>
    {
        public EntryFieldsViewModelTests()
            : base(new XElement("entry",
                new XElement("String",
                    new XElement("Key", "Title"),
                    new XElement("Value", "Not used")),
                new XElement("String",
                    new XElement("Key", "UserName"),
                    new XElement("Value", "Not used")),
                new XElement("String",
                    new XElement("Key", "Password"),
                    new XElement("Value", "Not used")),
                new XElement("String",
                    new XElement("Key", "Notes"),
                    new XElement("Value", "Not used")),
                new XElement("String",
                    new XElement("Key", "URL"),
                    new XElement("Value", "Not used")),
                new XElement("String",
                    new XElement("Key", "Normal Field"),
                    new XElement("Value", "Normal Field Value")),
                new XElement("String",
                    new XElement("Key", "Protected Field"),
                    new XElement("Value",
                        new XAttribute("Protected", "True"),
                        "Protected Field Value")))) {}

        [Fact]
        public void Should_detect_protection()
        {
            Populate();

            var normalField = ViewModel.Items
                .First(x => x.Key == "Normal Field");
            Assert.False(normalField.IsProtected);

            var protectedField = ViewModel.Items
                .First(x => x.Key == "Protected Field");
            Assert.True(protectedField.IsProtected);
        }

        [Fact]
        public void Should_include_non_standard_strings()
        {
            Populate();

            var fields = ViewModel.Items
                .ToDictionary(x => x.Key, x => x.Value);

            Assert.Equal("Normal Field Value", fields["Normal Field"]);
            Assert.Equal("Protected Field Value", fields["Protected Field"]);
        }

        [Fact]
        public void Should_not_include_standard_strings()
        {
            Populate();

            var fields = ViewModel.Items
                .Select(x => x.Key)
                .ToList();

            Assert.DoesNotContain("URL", fields);
            Assert.DoesNotContain("Title", fields);
            Assert.DoesNotContain("Notes", fields);
            Assert.DoesNotContain("UserName", fields);
            Assert.DoesNotContain("Password", fields);
        }

        [Fact]
        public void Should_update_IsExpanded_state_of_items()
        {
            var item1 = new EntryFieldItemViewModel();
            var item2 = new EntryFieldItemViewModel();

            ViewModel.Items.Add(item1);
            ViewModel.Items.Add(item2);

            ViewModel.SelectedItem = item1;
            Assert.True(item1.IsExpanded);
            Assert.False(item2.IsExpanded);

            ViewModel.SelectedItem = item2;
            Assert.False(item1.IsExpanded);
            Assert.True(item2.IsExpanded);
        }

        protected override void AssertValues(EntryFieldsViewModel viewModel)
        {
            Assert.Equal(2, viewModel.Items.Count);
        }

        protected override object GetLoadedIndicator(EntryFieldsViewModel viewModel)
        {
            return viewModel.Items.Any() ? new object() : null;
        }
    }
}