
import tensorflow as tf
from get_params import get_params
from BatchGenerator import BatchGenerator
from PredictiveCodingModel import PredictiveCodingModel


def test_create_batch():
    filename = 'tiny-shakespeare.txt'
    batch_generator = BatchGenerator(filename,10,2)

    texts = ["hello",'tensorflow']
    batch = batch_generator(texts)

    print batch


def test_length():
    with tf.Session() as session:
        filename = 'tiny-shakespeare.txt'
        batch_generator = BatchGenerator(filename, 100, 2)

        texts = ['hello tensorflow','goodluck you']
        for index,text in enumerate(texts):
            print "[{}] '{}': {} characters".format(index+1,text,len(text))

        batch = batch_generator(texts)
        batch = tf.convert_to_tensor(batch,tf.float32)

        pm = PredictiveCodingModel(get_params(),batch)
        print session.run([pm.length,pm.mask])


if __name__ == "__main__":
    # test_create_batch()
    test_length()
