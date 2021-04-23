#include <map>
#include "gtest/gtest.h"

using namespace::std;

TEST(MapTest, Constructor)
{
    map<char,int> map1 = {
        {'a',1},
        {'b',2},
        {'c',3},
    };

    ASSERT_EQ(map1.size(),3);
    ASSERT_EQ(map1['a'],1);
}