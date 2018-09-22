import numpy as np
from ppo.history import *


test = get_gae(np.asarray([0,0,0,0,0,0,0,0,0,0,0]), np.asarray([0,1,2,3,4,5,6,7,8,9,10]), 11)
test = get_gae(np.asarray([0,0,0,0,0,0,0,0,0,0,1]), np.asarray([0,0,0,0,0,0,0,0,0,0,0]), 0)