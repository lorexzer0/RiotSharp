﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RiotSharp.Endpoints.Interfaces;
using RiotSharp.Http.Interfaces;
using RiotSharp.Misc;

namespace RiotSharp.Endpoints.ChampionEndpoint
{
    /// <summary>
    /// Implementation of the IChampionEndpoint interface.
    /// </summary>
    /// <seealso cref="RiotSharp.Endpoints.Interfaces.IChampionEndpoint" />
    public class ChampionEndpoint : IChampionEndpoint
    {
        private const string PlatformRootUrl = "/lol/platform/v3";
        private const string ChampionsUrl = "/champions";
        private const string ChampionRotationUrl = "/champion-rotations";
        private const string IdUrl = "/{0}";

        private readonly IRateLimitedRequester _requester;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChampionEndpoint"/> class.
        /// </summary>
        /// <param name="requester">The requester.</param>
        public ChampionEndpoint(IRateLimitedRequester requester)
        {
            _requester = requester;
        }

        /// <inheritdoc />
        public async Task<List<Champion>> GetChampionsAsync(Region region, bool freeToPlay = false)
        {
            var json = await _requester.CreateGetRequestAsync(PlatformRootUrl + ChampionsUrl, region,
                new List<string> { $"freeToPlay={freeToPlay.ToString().ToLower()}" }).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ChampionList>(json).Champions;
        }

        /// <inheritdoc />
        public async Task<Champion> GetChampionAsync(Region region, int championId)
        {
            var json = await _requester.CreateGetRequestAsync(
                PlatformRootUrl + ChampionsUrl + string.Format(IdUrl, championId), region).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Champion>(json);
        }

        /// <inheritdoc />
        public async Task<ChampionRotation> GetChampionRotationAsync(Region region)
        {
            var json = await _requester.CreateGetRequestAsync(PlatformRootUrl + ChampionRotationUrl, region
                ).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ChampionRotation>(json);
        }
    }
}
