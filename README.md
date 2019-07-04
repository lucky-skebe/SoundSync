# SoundSync

**WIP WARING!**

**THIS VERSION IS VERY MUCH WORK IN PROGRESS AND SOME FEATURES REGRESSED DUE TO A MAYOR REFACTORING**

**IF YOU WANT THE VERSION WITH THE MOST USABLE FEATURES PLEASE USE THE COMMIT 58a1a03f3c58756a551ccde4cc59a2b2c0adc8b9 OR DOWNLOAD THE PRECOMPIPED VERSION FROM THE RELEASES TAB**

Ever wanted to play a game with buttplug vibration but the Game didn’t support it?
Ever wanted to watch a certain Video with vibration but there were no Scriptfiles for it?

Fear no more!

I created a little program for myself that just makes a buttplug vibrate according to the volume of your PCs output using the buttplug server.

##Feature List

Legend:
- [ ] Not yet implemented
- [x] Implemented and currently working
- [x] ~~Was implemented in previous wersion but is currently unavaiable~~ 

Planned Featres:
- [x] ~~Simple Server management~~
   - [x] ~~Connect/Disconnect to buttplug server~~
   - [x] ~~Start/Stop Scaning~~
   - [x] ~~Choose Single Buttplug Device~~
   - [x] Automatically connects to "ws://ws://localhost:12345/buttplug" (Default unsafe butplug port)
   - [ ] Embedded Server support
   - [ ] Server Management UI improvement.
 - [x] Vibration Scaling.
   - [x] ~~only linear between Min and Max values atm.~~
   - [x] Linear Scaling with a fixed 10x multiplier
- [ ] Pipeline
   - [ ] Multi Vibration Support
   - [ ] Support for user created Plugins (Custom inputs / outputs or manipulation of signal in the pipeline)
   - [ ] Working Pipeline UI (Connecting Elements via Drag & Drop, Controlling Element Parameters)
- [ ] Audio Device Selection
  - [x] currently only Loopback audio
- [ ] Save/Load Setting
- [ ] Pause/Resume

Optional Features (if I feel like it):
- [ ] Advanced Audio Scaling 
  - [ ] quadratic/logaritmic scaling
  - [ ] multiple thresholds
- [ ] JS/Electron Port for cross platform compartibility
- [ ] Grouping of PipeLine Elements to one container (e.g. having a multiply, add and clamping element in one group wich could behave like the old linear scaling behaviour)

##Technical details

In the latest version I started implementing an extensible Pipeline very heavily influenced by the GStreamer Framework wich should make it possible to create custom pipelines to you buttplug device (or other outputs in theory).
This should also make it possible to extend the functionality vie Plugins later on.

Ever wanted to control your philips hue together with your butplug based on your cpu load?

With the new pipeline and plugins you should be able to.

### Avaiable Pipeline Elements

 - Audio
  - [x] LoopbackAudioSrc - Input source based on the loopback audio volume
  - [ ] AudioSrc - Same as LoopbackAudioSrc but based on an input audio device of you system (a microphone for example) 
 - Buttplug
  - [x] ButtplugSink - Output sending the signal to your buttplug device (or devices)
 - Misc
  - [x] MultiplyElement - Multiply an input by a  fixed amount
  - [x] TimedAVGElement - Takes an average over the input signal in a fixed interval
  - [ ] ClampElement - Clamps the signal between two values (Min and Max value)
  - [ ] Tee and Merge - Splits the pipeline or merges it to be more flexible
