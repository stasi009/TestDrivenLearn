

import threading


def thread_obj_with_arguments():

    def worker(num):     print 'Worker: %s' % num

    threads = []
    for i in range(5):
        t = threading.Thread(target=worker, args=(i,))
        threads.append(t)
        t.start()

