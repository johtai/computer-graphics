#include "shapes.h"

const std::vector<GLfloat> tetrafigure {
    -1.0f, -1.0f, -1.0f, 0.0f, 0.0f, 1.0f,
        -1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f,
        1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f,

        -1.0f, -1.0f, -1.0f, 0.0f, 0.0f, 1.0f,
        -1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f,
        1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f,

        -1.0f, -1.0f, -1.0f, 0.0f, 0.0f, 1.0f,
        1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f,
        1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f,

        1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f,
        1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f,
        -1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f,
};

   
const std::vector<GLfloat> cubefigure{
    //ïîçèöèè             //öâåòà            //òåêñòóðíûå êîîðäèíàòû
    -1.0f,  1.0f, 1.0f,   0.0f, 1.0f, 0.0f,  0.0f, 0.1f,  
     1.0f,  1.0f, 1.0f,   1.0f, 0.0f, 0.0f,  1.0f, 1.0f,
     1.0f, -1.0f, 1.0f,   1.0f, 1.0f, 0.0f,  1.0f, 0.0f,
     -1.0f,-1.0f, 1.0f,   1.0f, 0.0f, 1.0f,  0.0f, 0.0f,

     -1.0f, 1.0f, -1.0f, 0.5f, 0.5f, 0.5f, 0.0f, 0.1f,
     -1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
     -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
     -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 0.0f,

     1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.1f,
     -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
     -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f,
     1.0f, -1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f,

     1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.1f,
     1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f,
     1.0f, -1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 1.0f, 0.0f,
     1.0f, -1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f,

     1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.1f,
     -1.0f, 1.0f, -1.0f, 0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
     -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f,
     1.0f, -1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f,

     1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.1f,
     -1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
     -1.0f, 1.0f, -1.0f, 0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
     1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f,

};

std::vector<float> hsvToRgb(float hue, float saturation = 100.0, float value = 100.0)
{
    int perc = (int)floor(hue / 60) % 6;

    float div = (100.0f - saturation) * value / 100.0;
    float prod = (value - div) * (((int)hue % 60) / 60.0);
    float sum = div + prod;
    float dec = value - prod;

    switch (perc)
    {
    case 0:
        return { value / 100.0f, sum / 100.0f, div / 100.0f, 1.0f };
    case 1:
        return { dec / 100.0f, value / 100.0f, div / 100.0f, 1.0f };
    case 2:
        return { div / 100.0f, value / 100.0f, sum / 100.0f, 1.0f };
    case 3:
        return { div / 100.0f, dec / 100.0f, value / 100.0f, 1.0f };
    case 4:
        return { sum / 100.0f, div / 100.0f, value / 100.0f, 1.0f };
    case 5:
        return { value / 100.0f, div / 100.0f, dec / 100.0f, 1.0f };
    }

    return { 0, 0, 0, 0 };
}


std::vector<GLfloat> circle{};
const float radius = 0.5f;

void initCircle() {
    // ������� ������
    circle.push_back(0.0f);
    circle.push_back(0.0f);
    circle.push_back(0.0f);

    // ���� ������
    circle.push_back(1.0f);
    circle.push_back(1.0f);
    circle.push_back(1.0f);
    circle.push_back(1.0f);

    std::vector<float> color;
    for (int i = 0; i < 360; ++i) {
        float angle = (i * 2.0f * 3.14159265) / 360;

        color = hsvToRgb(i % 360);

        circle.push_back(radius * cos(angle));
        circle.push_back(radius * sin(angle));
        circle.push_back(0.0f);

        circle.push_back(color[0]);
        circle.push_back(color[1]);
        circle.push_back(color[2]);
        circle.push_back(color[3]);
    }

    circle.push_back(radius * cos(0.0f));
    circle.push_back(radius * sin(0.0f));
    circle.push_back(0.0f);

    circle.push_back(color[0]);
    circle.push_back(color[1]);
    circle.push_back(color[2]);
    circle.push_back(color[3]);
}