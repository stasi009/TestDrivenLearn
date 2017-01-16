
from BatchGenerator import BatchGenerator


def test_create_batch():
    filename = 'tiny-shakespeare.txt'
    batch_generator = BatchGenerator(filename,10,2)

    texts = ["hello",'tensorflow']
    batch = batch_generator(texts)

    print batch


if __name__ == "__main__":
    test_create_batch()
