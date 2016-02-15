
import numpy as np
import matplotlib.pyplot as plt

def detect_peaks(v, change_ratio, x = None):
    """
    in our context, since we only care about spectrum
    so we can assume that, all "v" are positive
    """
    peaks = []
    max_ratio = 1 + change_ratio
    v = np.asarray(v) # if v is ndarray, it won't copy
    
    if x is None:
        x = np.arange(len(v))
        
    assert(len(v) == len(v))
        
    if (v[0]/v[1] > max_ratio):
        peaks.append((0,x[0],v[0]))
        
    for index in range(1,len(v)-1):
        if (v[index]/v[index-1] > max_ratio) and (v[index]/v[index+1] > max_ratio ):
            peaks.append((index,x[index],v[index]))
            
    if ( v[-1]/v[-2] > max_ratio):
        peaks.append((len(v)-1,x[-1],v[-1]))
        
    return np.asarray(peaks)
            
    
def demo_sampling_theorem():
    
    def sampling(fsample):
        N = 256
        dt = 1/fsample
        ns = dt * np.arange(N)
        
        w1 = 5
        ts1 = 0.5 * np.sin(w1 * ns + 100)
        
        w2 = 9
        ts2 = 3 * np.cos(w2*ns + 30)
        
        ts = 50 + ts1 + ts2
        print("fs=%f,f1=%f,f2=%f"%(fsample,w1/(2*np.pi),w2/(2*np.pi)))
        
        freq_signals = np.fft.rfft(ts)/N
        freq_spectrum = np.abs(freq_signals)
        freqs = np.fft.rfftfreq(ns.shape[-1],dt)   
        print("average=%f"%(freq_spectrum[0]))
        
        peak_freqs = detect_peaks(freq_spectrum,1,freqs)
        print("peak frequencies: %s"%(peak_freqs))
        
        plt.plot(freqs,freq_spectrum,"r*-")
        plt.show()    
        
    # sampling(1)
    sampling(10/np.pi)
    
if __name__ == "__main__":
    demo_sampling_theorem()
        