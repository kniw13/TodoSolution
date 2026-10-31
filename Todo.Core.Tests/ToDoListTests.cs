using Xunit;
using Todo.Core;
using System.Linq;
using System.IO;
using System;

namespace Todo.Core.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void Add_IncreasesCount()
        {
            var list = new TodoList();
            list.Add("  task  ");
            Assert.Equal(1, list.Count);
            Assert.Equal("task", list.Items.First().Title);
        }

        [Fact]
        public void Remove_ById_Works()
        {
            var list = new TodoList();
            var item = list.Add("a");
            Assert.True(list.Remove(item.Id));
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public void Find_ReturnsMatches()
        {
            var list = new TodoList();
            list.Add("Buy milk");
            list.Add("Read book");
            var found = list.Find("buy").ToList();
            Assert.Single(found);
            Assert.Equal("Buy milk", found[0].Title);
        }

        [Fact]
        public void SaveAndLoad_PersistsData()
        {
            // Arrange
            var list = new TodoList();
            var item1 = list.Add("Task 1");
            item1.MarkDone();
            var item2 = list.Add("Task 2");

            string tempFile = Path.GetTempFileName();

            try
            {

                list.Save(tempFile);

                var loadedList = new TodoList();
                loadedList.Load(tempFile);


                Assert.Equal(2, loadedList.Count);

                var loadedItem1 = loadedList.Items.First(i => i.Id == item1.Id);
                Assert.Equal(item1.Title, loadedItem1.Title);
                Assert.True(loadedItem1.IsDone);

                var loadedItem2 = loadedList.Items.First(i => i.Id == item2.Id);
                Assert.Equal(item2.Title, loadedItem2.Title);
                Assert.False(loadedItem2.IsDone);
            }
            finally
            {

                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
    }
}