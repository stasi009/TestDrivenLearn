
import numpy as np
import matplotlib.pyplot as plt

fsample = 8
dt = 1/fsample

N = 256
ns = dt * np.arange(N)

f1 = 1
ts1 = 2 * np.sin(2 * np.pi * f1 * ns - np.pi/2)

f2 = 2
ts2 = 6 * np.cos(2 * np.pi * f2*ns )

ts = 5 + ts1 + ts2
print("fs=%f,f1=%f,f2=%f"%(fsample,f1,f2))

freq_signals = np.fft.rfft(ts)
freq_spectrum = np.abs(freq_signals) * 2/N
freq_spectrum[0] /= 2

freqs = np.fft.rfftfreq(ns.shape[-1],dt)   
print("average=%f"%(freq_spectrum[0]))

plt.plot(freqs,freq_spectrum,"r*-")
plt.xlim(-1,5)
plt.ylim(0,6)
plt.show()    