﻿namespace BoardGameGeek.Dungeon
{
    public class CollectionDto
    {
        public int GameId { get; set; }
        public string GameName { get; set; }
        public string Image { get; set; }
        public string Thumbnail { get; set; }
        public int TotalPlays { get; set; }
        public string Comments { get; set; }

        public override string ToString()
        {
            return $"{GameName} (\u03a3 {TotalPlays}x)";
        }
    }
}
