using System.Text.Json.Serialization;
using HeySiri.Core.SystemTextJson;
using HeySiri.Core.Extentions;
using HeySiri.Core.Reporters;

namespace HeySiri.Core.Bandwagon;

public class LiveServiceInfo : IReportable
{
    [JsonPropertyName("ve_status")]
    public LiveServiceInfoStatus Status { get; set; }

    [JsonPropertyName("ssh_port")]
    public int SshPort { get; set; }

    [JsonPropertyName("veid")]
    public int Id { get; set; }

    [JsonPropertyName("ip_addresses")]
    public List<string> IPAddresses { get; set; }

    [JsonPropertyName("mem_available_kb")]
    public int MemAvailableKB { get; set; }

    public long MemUnavailableB => PlanRam - MemAvailableKB * 1024;

    [JsonPropertyName("plan_ram")]
    public long PlanRam { get; set; }

    [JsonPropertyName("plan_disk")]
    public long PlanDisk { get; set; }

    [JsonPropertyName("ve_used_disk_space_b")]
    public long UsedDiskSpaceB { get; set; }

    [JsonPropertyName("swap_total_kb")]
    public int SwapTotalKB { get; set; }

    [JsonPropertyName("swap_available_kb")]
    public int SwapAvailableKB { get; set; }

    public int SwapUnavailableKB { get; set; }

    [JsonPropertyName("plan_monthly_data")]
    public long PlanMonthlyData { get; set; }

    [JsonPropertyName("data_counter")]
    public long DataCounter { get; set; }

    [JsonPropertyName("data_next_reset")]
    [JsonConverter(typeof(UnixDateTimeOffsetConverter))]
    public DateTimeOffset DataNextReset { get; set; }

    [JsonPropertyName("os")]
    public string OS { get; set; }

    [JsonPropertyName("node_location")]
    public string NodeLocation { get; set; }

    [JsonPropertyName("node_location_id")]
    public string NodeLocationId { get; set; }

    [JsonPropertyName("node_datacenter")]
    public string NodeDataCenter { get; set; }

    [JsonPropertyName("plan")]
    public string Plan { get; set; }

    [JsonPropertyName("hostname")]
    public string HostName { get; set; }

    public string GetReport()
    {
        try
        {
            return $@"Plan: {Plan}
Host Name: {HostName}
Data Center: {NodeDataCenter}
VPS ID: {Id}
IP Address: {string.Join(", ", IPAddresses)}
SSH Port: {SshPort}
Status: {Status}
RAM Usage(MB): {MemUnavailableB.ByteToMebi():N2}/{PlanRam.ByteToMebi():N2} {(MemUnavailableB * 1.0 / PlanRam):P2}
SWAP Usage(MB): {SwapUnavailableKB.KibiToMebi():N2}/{SwapTotalKB.KibiToMebi():N2} {(SwapUnavailableKB * 1.0 / SwapTotalKB):P2}
Disk Usage(GB): {UsedDiskSpaceB.ByteToGibi():N2}/{PlanDisk.ByteToGibi():N2} {(UsedDiskSpaceB * 1.0 / PlanDisk):P2}
Bandwidth Usage(GB): {DataCounter.ByteToGibi():N2}/{PlanMonthlyData.ByteToGibi():N2} {(DataCounter * 1.0 / PlanMonthlyData):P2}
Bandwidth Resets: {DataNextReset}
OS: {OS}";
        }
        catch (Exception e)
        {
            throw new ArgumentException("Could not generic report, please check the response.", e);
        }
    }
}

public enum LiveServiceInfoStatus
{
    Running,
}