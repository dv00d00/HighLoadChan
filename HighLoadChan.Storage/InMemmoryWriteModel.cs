namespace HighLoadChan.Storage
{
    using System;
    using System.Collections.Generic;

    using HighLoadChan.Core;

    public class InMemmoryWriteModel : IWriteModel
    {
        private readonly IMessanger messanger;

        private int currentPostId;
        private int currentThreadId;

        public InMemmoryWriteModel(IMessanger messanger)
        {
            this.messanger = messanger;
            this.messanger.RegisterCommandHandler<AddPostCommand>(this);
            this.messanger.RegisterCommandHandler<AddThreadCommand>(this);
        }

        public void HandleCommand(AddPostCommand addPostCommand)
        {
            this.currentPostId ++;

            var post = addPostCommand.Post;

            this.messanger.SendEvent(new PostAddedEvent
                {
                    Post = new Post
                        {
                            Author = post.Author,
                            Content = post.Content,
                            Created = DateTime.UtcNow,
                            Id = this.currentPostId,
                            ThreadId = post.ThreadId
                        }
                });
        }

        public void HandleCommand(AddThreadCommand addThreadCommand)
        {
            this.currentThreadId++;

            var thread = addThreadCommand.Thread;

            this.messanger.SendEvent(new ThreadAddedEvent
                {
                    Thread = new Thread
                        {
                            BoardName = thread.BoardName,
                            Created = DateTime.UtcNow,
                            Id = this.currentThreadId,
                            Name = thread.Name,
                            Posts = new List<Post>()
                        }
                });
        }
    }
}