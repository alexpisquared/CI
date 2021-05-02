using System.Collections.Generic;

namespace RdpFacility
{
  internal class SpeechSynthesizer
  {
    public static IReadOnlyList<VoiceInformation> AllVoices { get; internal set; } = new List<VoiceInformation>();
  }
}