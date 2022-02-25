﻿namespace DisneyApi.Dto
{
    public class CharacterCreationDto
    {
        public string imageUrl { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; } = 0;
        public string Role { get; set; } = string.Empty;

        public string Story { get; set; } = string.Empty;

    }
}
