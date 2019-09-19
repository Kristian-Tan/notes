```
val batteryStatus = this.applicationContext.registerReceiver(null, IntentFilter(Intent.ACTION_BATTERY_CHANGED))
val batteryLevel = batteryStatus!!.getIntExtra(BatteryManager.EXTRA_LEVEL, -1)
val batteryScale = batteryStatus!!.getIntExtra(BatteryManager.EXTRA_SCALE, -1)
val batteryPercent = Math.round( ( batteryLevel.toDouble() / batteryScale.toDouble() * 100.0) )
Log.d("BATTERYK", batteryPercent.toString())
```