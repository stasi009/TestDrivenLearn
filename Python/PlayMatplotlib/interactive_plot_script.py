
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

# switch to interactive plot, plot won't embedded in PTVS
# %matplotlib 
# if want to switch back to inline plot
# %matplotlib inline

fig = plt.figure()# create a new figure
ax1 = fig.add_subplot(2, 2, 1)
ax2 = fig.add_subplot(2, 2, 2)
ax3 = fig.add_subplot(2, 2, 3)

# draw on the last figure and subplot
plt.plot(np.random.randn(50).cumsum(), 'k--')

# returned ax object represents the subplot, can be plot on
_ = ax1.hist(randn(100), bins=20, color='k', alpha=0.3)
ax2.scatter(np.arange(30), np.arange(30) + 3 * randn(30))

# interactive plot needs manual refresh
# or you don't need to type "draw", but resize the window to force a redraw
plt.draw()


plt.close('all')# close the window