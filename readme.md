## About

HackOS is an offensive cyber-security sandbox game. Very much proof-of-concept and far from production-ready.

This is a participating project of the 6th InnoShow held by the Faculty of Engineering, The University of Hong Kong.
https://innoacademy.engg.hku.hk/hack/

[Game Trailer](https://youtu.be/_CbzQMBAphA)

### Basic Overview
- 3 apps Terminal, Zignal, Bravo, all can be launched by clicking the dock icons on the left of Desktop.
- Terminal is where hack actions are done: entering hacking commands, mutating game states, etc.
- Zignal is the "mission control", for selecting different jobs. It also provides job context with a instant-messaging-app-like interface.
- Bravo is the shop for hack tools.

## Setup
HackOS requires save file to function properly even for the first run: `SaveData.json`.
It should come with the built but if it did not, then create one with the content:
```json
{"money":999.0,"exp":0,"hackTools":["about","exit","cd","clear","help","ls","nslookup","pwd","ssh","whoami","bleed","hydra","voler","nmap"]}
```
this JSON string gives 999 money and unlocks all commands.

The game uses Unity's default presistent save path. If not sure, launch the game once, and hit the game menu button (most top left button), then choose "Save". A `SaveData.json` will then created. Then use your computer search program to locate it.

## Solution to Jobs
### Crack Addict:
1. nslookup hcku.hk
2. nmap 147.8.2.58
3. hydra -l root -P wordlists/rockyou.txt 147.8.2.58 ssh
4. ssh root@147.8.2.58
5. voler -l root -p toor 147.8.2.58 /home/root/comp3329/exam.pdf
### Heart Surgeon:
1. bleed 172.245.143.198
2. look up `[username]::[password]` combo from the gibberish
3. repeat: press y + enter in the "Beat more?" prompt until you have found 3 pairs of credentials
