using System;
using System.Collections.Generic;

namespace HighLoadChan.Storage
{
    using System.Collections.Concurrent;

    using HighLoadChan.Core;

    using Thread = HighLoadChan.Core.Thread;

    public class InMemmoryReadModel : IReadModel
    {
        private readonly ConcurrentDictionary<int, Thread> threads = new ConcurrentDictionary<int, Thread>();
        private readonly ConcurrentDictionary<int, Thread> shortThreads = new ConcurrentDictionary<int, Thread>();

        public InMemmoryReadModel(IMessanger messanger)
        {
            messanger.RegisterEventHandler<PostAddedEvent>(this);
            messanger.RegisterEventHandler<ThreadAddedEvent>(this);
        }

        public void HandleEvent(ThreadAddedEvent threadAddedEvent)
        {
            var thread = threadAddedEvent.Thread;

            this.threads.AddOrUpdate(thread.Id, i => thread, (i, x) => { throw new Exception(); });
        }

        public void HandleEvent(PostAddedEvent postAddedEvent)
        {
            var post = postAddedEvent.Post;

            if (threads.ContainsKey(post.ThreadId))
            {
                threads[post.ThreadId].Posts.Add(post);
            }

            if (shortThreads.ContainsKey(post.ThreadId))
            {
                var thread = shortThreads[post.ThreadId];

                if (thread.Posts.Count > 4)
                {
                    thread.Posts = new List<Post>
                        {
                            thread.Posts[0],
                            thread.Posts[thread.Posts.Count - 1 - 2],
                            thread.Posts[thread.Posts.Count - 1 - 1],
                            thread.Posts[thread.Posts.Count - 1],
                        };
                }
            }
        }

        public Thread GetWholeThread(int threadId)
        {
            return threads[threadId];
        }

        public BoardPageModel GetBoardPage(int page, string board)
        {
            return new BoardPageModel { Threads = shortThreads.Values };
        }
    }
}
