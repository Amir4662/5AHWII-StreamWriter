
using Microsoft.AspNetCore.Mvc;

namespace StreamWriter.Controllers
{
    [Route("daily-challenge")]
    [ApiController]
    public class DailyChallengeController : ControllerBase
    {
        private readonly IDailyChallengeTextGenerator _dailyChallengeTextGenerator;

        public DailyChallengeController(
            IDailyChallengeTextGenerator dailyChallengeTextGenerator)
        {
            _dailyChallengeTextGenerator = dailyChallengeTextGenerator;
        }

        [HttpGet("text")]
        public async Task<ActionResult<string>> GetDailyChallengeText()
        {
            string text = await _dailyChallengeTextGenerator.GetText();
            return Ok(text);
        }
    }

    public interface IDailyChallengeTextGenerator
    {
        Task<string> GetText();
    }

    public class StaticDailyChallengeTextGenerator : IDailyChallengeTextGenerator
    {
        public Task<string> GetText()
        {
            return Task.FromResult(
                "This is a static text from StaticDailyChallengeTextGenerator.");
        }
    }

    public class RotatingDailyChallengeTextGenerator : IDailyChallengeTextGenerator
    {
        private readonly string[] _texts;
        private int _textIndex;
        private readonly object _lock = new();

        public RotatingDailyChallengeTextGenerator(string[] texts)
        {
            if (texts == null || texts.Length == 0)
            {
                throw new ArgumentException(
                    "At least one text must be provided.", nameof(texts));
            }

            _texts = texts;
        }

        public Task<string> GetText()
        {
            lock (_lock)
            {
                string result = _texts[_textIndex];
                _textIndex = (_textIndex + 1) % _texts.Length;

                return Task.FromResult(result);
            }
        }
    }

    public class DelayDailyChallengeTextGenerator : IDailyChallengeTextGenerator
    {
        private readonly TimeSpan _delay;
        private readonly IDailyChallengeTextGenerator _inner;

        public DelayDailyChallengeTextGenerator(
            TimeSpan delay,
            IDailyChallengeTextGenerator inner)
        {
            _delay = delay;
            _inner = inner;
        }

        public async Task<string> GetText()
        {
            await Task.Delay(_delay);
            return await _inner.GetText();
        }
    }
}