# SoundSync

Ever wanted to play a game with buttplug vibration but the Game didn’t support it?
Ever wanted to watch a certain Video with vibration but there were no Scriptfiles for it?

Fear no more!

I created a little program for myself that just makes a buttplug vibrate according to the volume of your PCs output using the buttplug server.

Implemented:
- Simple Server management
   - Connect/Disconnect to buttplug server
   - Start/Stop Scaning
   - Choose Single Buttplug Device
 - Vibration Scaling.
   - only linear between Min and Max values atm.

Planned Features:
- Embedded Server support
- Server Management UI improvement.
- Audio Device Selection
  - currently only Loopback audio
- Save/Load Setting
- Pause/Resume

More Planned Features (if I feel like it):
- Advanced Audio Scaling 
  - quadratic/logaritmic scaling
  - multiple thresholds
- Multi Vibration Support
- JS/Electron Port for cross platform compartibility
