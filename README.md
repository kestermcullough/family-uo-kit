# Family UO kit

Small, family-friendly overlay for [ServUO](https://github.com/ServUO/ServUO)—not a server or client download.

## Useful commands

Use in-game as Owner/GM; creation, saving/restarting, and world setup require the appropriate staff rank. Target commands: type, then click.

| Command | Use |
| --- | --- |
| `[CreateWorld` | Setup menu: decorations, doors, gates, teleporters, NPC/monster spawners, containers and dungeons. Select compatible, ungenerated entries; wait at least five minutes for containers. |
| `[CreateWorld nogump` | Run all setup immediately; menu recommended for expansion compatibility. |
| `[Save` / `[Restart` | Save / save and restart (disconnects everyone). |
| `[FamilyHelp` | Show custom family commands. |
| `[GiveSkills` / `[GiveSkills 100` / `[GiveSkills 120` | Set all targeted player skills; default 100. |
| `[GiveStats` / `[GiveStats 100` / `[GiveStats 125` | Set targeted player's STR/DEX/INT; default 100. |
| `[GiveGold` / `[GiveGold 1000000` | Bank check for targeted player; default 100,000. |
| `[MakeGM` | Promote targeted player to GM (Owner only). |
| `[Add` | Searchable item/creature browser. |
| `[Add Gold 10000` / `[Add Bandage 100` | Create stacks. |
| `[Add Longsword` / `[Add LeatherChest` / `[Add BankCheck 1000000` | Create equipment/check. |
| `[Props` / `[Remove` | Inspect/edit / delete targeted object. |
| `[Who` | Select online player: **Go to them** / **Bring them here**. |
| `[Tele` / `[Move` / `[Go` | Teleport to clicked spot / move targeted object or creature / destinations menu. |
| `[Set Movable False` / `[Set Movable True` | Prevent pickup and normal ground decay / undo; save afterward. |
| `[Set IsBonded True` | Manually bond a targeted pet; normally unnecessary with this kit. |
| `[Add AnimalTrainer` | Place a stable NPC where convenient. |
| `[Add EtherealHorse` | Backpack mount statue: double-click to ride, dismount to pack away. |
| `[Add HitchingPost` | House stabling; lock it down, then say `stable` / `claim`. |
| `[Set UsesRemaining 1000000` | Give a targeted hitching post many uses. |
| `I wish to lock this down` | House-owner speech: target an item in your house. |
| `all stay` | Park pets; not protection from attacks. |
| `stable` / `claim` | Stable/retrieve pets near a trainer or locked-down house hitching post. |

World setup already runs `[Decorate`, `[DoorGen`, and `[XmlLoad Spawns`; don't run them separately unless needed. `[DeleteWorld` removes generated content—**destructive**. CreateWorld tracks generated sections, but separate generators can duplicate content.

## Changes / custom features

Defaults in [`Config/Family.cfg`](Config/Family.cfg):

- **Peaceful mobs:** wild creatures don't initiate normal combat; may retaliate. Pets still obey attack orders. Not invulnerability or protection from every scripted encounter.
- **Instant bonding:** eligible player pets bond on taming/transfer and startup, including existing pets. Summons and unbondable creatures excluded.
- **Perfect pet care:** maximum loyalty, hunger/thirst reset on acquisition/startup and hourly maintenance, no loyalty loss or dead-pet abandonment. Feeding isn't required for loyalty; normal control-skill checks and combat still apply.
- **Mounted mining:** mine without dismounting. No automatic area/self-target mining added.
- **No Magic Storm relocation:** disables the login-time evacuation popup in the affected regions.

Also: custom boost/GM commands; first automatically created account becomes Owner; 10 accounts per IP; SHA-512 password protection; 15-minute saves and hourly archives retained 30 days; precompiled scripts and macOS/Mono build fixes. World generation, ethereal mounts and hitching posts are existing ServUO features, not additions.

## Install

Requires Git, .NET 8 SDK, a .NET Framework-compatible runtime (Mono on macOS/Linux), and your own legal UO client data. Tested against ServUO commit `aa4c139fd64f1a2445ec186a134f0e2f2450ba53`; other versions may need patch adjustments. Start with a fresh checkout, or back up existing saves/configs first.

```sh
git clone https://github.com/kestermcullough/family-uo-kit.git
git clone https://github.com/ServUO/ServUO.git servuo
cd servuo
git checkout aa4c139fd64f1a2445ec186a134f0e2f2450ba53
git apply --check ../family-uo-kit/patches/family-friendly.patch
git apply ../family-uo-kit/patches/family-friendly.patch
mkdir -p Scripts/Custom
cp ../family-uo-kit/Config/Family.cfg Config/
cp ../family-uo-kit/Scripts/Custom/*.cs Scripts/Custom/
```

Set `CustomPath` in `Config/DataPath.cfg` to your client data directory. Review `Config/Server.cfg` (name, listen/address, TCP port **2593**). Archives default to `Backups/Archived`; optionally set `ArchivesPath` elsewhere.

```sh
dotnet build -c Release
mono ServUO.exe -service
```

On Windows, build with `dotnet build -c Release` and run `ServUO.exe`. Connect ClassicUO/Razor to the server's LAN/VPN address, port 2593. First login creates Owner; run `[CreateWorld` once to populate the world. Restart after code/config changes. Keep private shards behind a trusted LAN/VPN: create your Owner account **before** allowing anyone else to connect.

No accounts, saves, logs, credentials, personal IPs, client assets, or machine-specific paths included. GPL-2.0; see [LICENSE](LICENSE). Upstream credit: ServUO contributors.
