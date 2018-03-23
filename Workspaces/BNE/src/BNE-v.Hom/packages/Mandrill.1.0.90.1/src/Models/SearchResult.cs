﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchResult.cs" company="">
//   
// </copyright>
// <summary>
//   The search result state.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Models
{
  /// <summary>
  ///   The search result state.
  /// </summary>
  public enum SearchResultState
  {
    /// <summary>
    ///   The sent.
    /// </summary>
    Sent,

    /// <summary>
    ///   The bounced.
    /// </summary>
    Bounced,

    /// <summary>
    ///   The rejected.
    /// </summary>
    Rejected,

    /// <summary>
    ///   The soft bounced.
    /// </summary>
    [EnumMember(Value = "soft-bounced")] SoftBounced,

    /// <summary>
    ///   The spam.
    /// </summary>
    Spam,

    /// <summary>
    ///   The unsub.
    /// </summary>
    Unsub,

    /// <summary>
    ///   The deferred.
    /// </summary>
    Deferred
  }

  /// <summary>
  ///   The search result.
  /// </summary>
  public class SearchResult
  {
    #region Public Properties

    /// <summary>
    ///   Gets or sets the _id.
    /// </summary>
    [JsonProperty("_id")]
    public string Id { get; set; }

    /// <summary>
    ///   Gets or sets the bounce_description.
    /// </summary>
    [JsonProperty("bounce_description")]
    public string BounceDescription { get; set; }

    /// <summary>
    ///   Gets or sets the clicks.
    /// </summary>
    [JsonProperty("clicks")]
    public int Clicks { get; set; }

    /// <summary>
    ///   Gets or sets the diag.
    /// </summary>
    [JsonProperty("diag")]
    public string Diag { get; set; }

    /// <summary>
    ///   Gets or sets the email.
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; }

    /// <summary>
    ///   Gets or sets the metadata.
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, string> Metadata { get; set; }

    /// <summary>
    ///   Gets or sets the opens.
    /// </summary>
    [JsonProperty("opens")]
    public int Opens { get; set; }

    /// <summary>
    ///   Gets or sets the sender.
    /// </summary>
    [JsonProperty("sender")]
    public string Sender { get; set; }

    /// <summary>
    ///   Gets or sets the smtp_events.
    /// </summary>
    [JsonProperty("smtp_events")]
    public IEnumerable<SmtpEvent> SmtpEvents { get; set; }

    /// <summary>
    ///   Gets or sets the state.
    /// </summary>
    [JsonProperty("state")]
    [JsonConverter(typeof (StringEnumConverter))]
    public SearchResultState State { get; set; }

    /// <summary>
    ///   Gets or sets the subject.
    /// </summary>
    [JsonProperty("subject")]
    public string Subject { get; set; }

    /// <summary>
    ///   Gets or sets the tags.
    /// </summary>
    [JsonProperty("tags")]
    public string[] Tags { get; set; }

    /// <summary>
    ///   Gets or sets the ts.
    /// </summary>
    [JsonProperty("ts")]
    public int Ts { get; set; }

    /// <summary>
    ///   Gets or sets the open details.
    /// </summary>
    /// <value>The open details.</value>
    [JsonProperty("opens_detail")]
    public IEnumerable<OpenDetail> OpenDetails { get; set; }

    /// <summary>
    ///   Gets or sets the click details.
    /// </summary>
    /// <value>The click details.</value>
    [JsonProperty("clicks_detail")]
    public IEnumerable<ClickDetail> ClickDetails { get; set; }

    #endregion
  }
}