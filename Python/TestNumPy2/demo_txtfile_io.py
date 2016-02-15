
import numpy as np

def demo_savetxt():
    a = np.random.randn(3,3)
    np.savetxt("test.txt",a,fmt="%3.2f",delimiter=",")
    
if __name__ == "__main__":
    demo_savetxt()
