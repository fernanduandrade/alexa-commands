namespace FunctionTypes

type TweetData =
    {
        edit_history_tweet_ids: string list;
        id: string;
        text: string
    }

type TweetMeta = 
    {
        newest_id: string;
        oldest_id: string;
        result_count: int;
        next_token: string
    }

type TweetResponse = 
    {
        data: TweetData list;
        meta: TweetMeta
    }