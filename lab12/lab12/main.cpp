#include "shapes.h"

const GLuint WIDTH = 800, HEIGHT = 800;

const char* VertexShaderSource = R"(
	        #version 330 core
	        in vec3 coord;
	        in vec3 colorVertex; //Цвет вершины (для градиента)
            uniform mat4 matr;
	        out vec3 vertexColor; //Передача цвета в фрагментный шейдер


	        void main() {
		        gl_Position = matr *  vec4(coord, 1.0f);
                vertexColor = colorVertex;
                
		       
	        }
        )";


const char* FragmentGradientShaderSource = R"(
	        #version 330 core
            in vec3 vertexColor;
            in vec2 TexCoord;            
            
            out vec4 color;

            void main()
            {
                color = vec4(vertexColor, 1.0);
            }
        )";


const char* VertexTexShaderSource = R"(
	        #version 330 core
	        layout (location = 0) in vec3 coord;
	        layout (location = 1) in vec3 colorVertex; //Цвет вершины (для градиента)
            layout (location = 2) in vec2 texCoord;

            out vec2 TexCoord;
            out vec3 VertexColor;

            uniform mat4 matr;

	        void main() {
		        gl_Position = matr *  vec4(coord, 1.0f);
                TexCoord = texCoord;
                VertexColor = colorVertex;
                      
	        }
        )";

const char* FragmentGradientTextureShaderSource = R"(
        #version 330 core
        in vec2 TexCoord; 
        in vec3 VertexColor;

        out vec4 color;

        uniform sampler2D ourTexture;

        uniform float colorAlpha;
        void main()
        {
            color =  mix( texture(ourTexture, TexCoord), vec4(VertexColor, 1.0f), colorAlpha);
        }
)";

const char* FragmentMixTextureShaderSource = R"(
        #version 330 core
        in vec2 TexCoord; 
        in vec3 VertexColor;

        out vec4 color;

        uniform sampler2D ourTexture;
        uniform sampler2D ourTexture2;

        uniform float colorAlpha;
        void main()
        {
            color =  mix(texture(ourTexture, TexCoord), texture(ourTexture2, TexCoord), 0.0f);
        }
)";

//projection* view* model* vec4(coord, 1.0f);


// Компиляция шейдера
GLuint compileShader(const GLchar* source, GLenum type) {
    GLuint shader = glCreateShader(type);
    glShaderSource(shader, 1, &source, nullptr);
    glCompileShader(shader);

    GLint success;
    glGetShaderiv(shader, GL_COMPILE_STATUS, &success);
    if (!success) {
        char infoLog[512];
        glGetShaderInfoLog(shader, 512, nullptr, infoLog);
        std::cerr << "ERROR::SHADER::COMPILATION_FAILED\n" << infoLog << std::endl;
    }

    return shader;
}


GLuint loadTexture(const char* filePath) {
    int width, height;
    unsigned char* image = SOIL_load_image(filePath, &width, &height, 0, SOIL_LOAD_RGB);
    if (!image) {
        std::cerr << "Failed to load texture" << std::endl;
        return 0;
    }

    GLuint texture;
    glGenTextures(1, &texture);
    glBindTexture(GL_TEXTURE_2D, texture);

    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, image);
    glGenerateMipmap(GL_TEXTURE_2D);

    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

    SOIL_free_image_data(image);
    glBindTexture(GL_TEXTURE_2D, 0);

    return texture;
}

// Создание шейдера
GLuint createShaderProgram(int filename) 
{
    GLuint vertexShader;
    if (filename == 1 || filename == 2) 
    {
        //Вертексный шейдер и проверка на ошибку создания
        vertexShader = compileShader(VertexTexShaderSource, GL_VERTEX_SHADER);
    }
    else 
    {
        vertexShader = compileShader(VertexShaderSource, GL_VERTEX_SHADER);
    }
    GLint success;
    GLchar infolog[512];
    glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
    if (!success) 
    {
        glGetShaderInfoLog(vertexShader, 512, nullptr, infolog);
        std::cout << "Error compiling vertex shader: " << infolog << std::endl;
    }
    GLuint fragmentShader;
    if (filename == 1)
    {
        //Фрагментный шейдер и проверка на ошибку создания
        fragmentShader = compileShader(FragmentGradientTextureShaderSource, GL_FRAGMENT_SHADER);
    }
    else if (filename == 2)
    {
        //Фрагментный шейдер и проверка на ошибку создания
        fragmentShader = compileShader(FragmentMixTextureShaderSource, GL_FRAGMENT_SHADER);
    }
    else if(filename == 0 || filename == 3 || filename == 4)
    {
        //Фрагментный шейдер и проверка на ошибку создания
        fragmentShader = compileShader(FragmentGradientShaderSource, GL_FRAGMENT_SHADER);
    }
        
    glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);
    if (!success) 
    {
        glGetShaderInfoLog(vertexShader, 512, nullptr, infolog);
        std::cout << "Error compiling fragment shader: " << infolog << std::endl;
    }

    GLuint shaderProgram = glCreateProgram();
    glAttachShader(shaderProgram, vertexShader);
    glAttachShader(shaderProgram, fragmentShader);
    glLinkProgram(shaderProgram);

    return shaderProgram;
}

// Создает фигуру в VBO по вершинам
void getShape(GLuint& VBO, GLuint count) 
{
    glGenBuffers(1, &VBO);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    if (count == 0 || count == 1 || count == 2) 
    {
        glBufferData(GL_ARRAY_BUFFER, cubefigure.size() * sizeof(GLfloat), cubefigure.data(), GL_STATIC_DRAW);
    }
    else if (count == 3) 
    {
        glBufferData(GL_ARRAY_BUFFER, tetrafigure.size() * sizeof(GLfloat), tetrafigure.data(), GL_STATIC_DRAW);
    }
    else if (count == 4)
    {
        initCircle();
        glBufferData(GL_ARRAY_BUFFER, circle.size() * sizeof(GLfloat), circle.data(), GL_STATIC_DRAW);
    }

    glBindBuffer(GL_ARRAY_BUFFER, NULL);
}

int main() 
{
    int width, height;
    width = 512; height = 512;

    sf::RenderWindow window(sf::VideoMode(WIDTH, HEIGHT), "Window");
    glewInit();

    GLuint VBO;
    GLuint count = 0;

    glEnable(GL_DEPTH_TEST);  //Включаем проверку глубины
    GLuint shaderProgram = createShaderProgram(count);

    // Вытягиваем ID атрибута вершин из собранной программы
    GLuint Attrib_vertex = glGetAttribLocation(shaderProgram, "coord");
    GLuint Color_vertex = glGetAttribLocation(shaderProgram, "colorVertex");
    GLuint Tex_Vertex;
    getShape(VBO, count);
    glm::mat4 model = glm::mat4(1.0f);
    //model = glm::translate(model, glm::vec3(0.0f, 0.0f, 0.0f));
    glm::mat4 view = glm::lookAt(
        glm::vec3(5.0f, 5.0f, -5.0f),//Положение 
        glm::vec3(0.0f, 0.0f, 0.0f),//Куда должна быть направлена камера
        glm::vec3(0.0f, 1.0f, 0.0f) //Для ориентации
    );

    glm::mat4 projection = glm::perspective(glm::radians(45.0f), (GLfloat)WIDTH / (GLfloat)HEIGHT, 0.1f, 100.0f);

    glm::mat4 mvp;
    mvp = projection * view * model;
    GLfloat moveX = 0.0f;
    GLfloat moveY = 0.0f;
    GLfloat intensity = 0.5f;

    //Создадим текстуру
    GLuint texture = loadTexture("container.jpg");
    GLuint texture2 = loadTexture("tex2.png");
    while (window.isOpen()) 
    {
        sf::Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed) { window.close(); }
            else if (event.type == sf::Event::MouseButtonPressed) 
            {               
                count = (count + 1) % 5;
                shaderProgram = createShaderProgram(count); 
                if (count == 1) 
                {
                    Tex_Vertex = glGetAttribLocation(shaderProgram, "texCoord");
                    //std::cout << Tex_Vertex;
                   
                   
                    //glUniform1f(colorAlphaLoc, 1.0f);
                }
                    
                getShape(VBO, count);
            }


            if (sf::Keyboard::isKeyPressed(sf::Keyboard::E)) 
            {
                //if(count == 1 && intensity <= 1)
                    intensity += 0.09f;
            }
               

            if (sf::Keyboard::isKeyPressed(sf::Keyboard::Q)) 
            {
                //if(count == 1 && intensity >= 0)
                    intensity -= 0.09f;
            }
    
        
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::Left))
            {              
                moveX = -0.5f;
                model = glm::translate(model, glm::vec3(moveX, 0.0f, 0.0f));
            }
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::Right)) 
            {
                moveX = 0.5f;
                model = glm::translate(model, glm::vec3(moveX, 0.0f, 0.0f));             
            }
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::Up)) 
            {
                moveY = 0.5f;
                model = glm::translate(model, glm::vec3(0.0f, moveY, 0.0f));
            }
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::Down))
            {
                moveY = -0.5f;
                model = glm::translate(model, glm::vec3(0.0f, moveY, 0.0f));
            }
        }
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        mvp = projection * view * model;
        glUseProgram(shaderProgram);
        glBindBuffer(GL_ARRAY_BUFFER, VBO);

        //Получаем расположение юниформ-переменных в Вертексном шейдере
        GLuint matrloc = glGetUniformLocation(shaderProgram, "matr");
        GLfloat colorAlphaLoc = glGetUniformLocation(shaderProgram, "colorAlpha");

        //Передаём юниформ-переменные в шейдеры
        glUniformMatrix4fv(matrloc, 1, GL_FALSE, glm::value_ptr(mvp));
        glUniform1f(colorAlphaLoc, intensity);
     
        if (count == 0) 
        {
            // Считываем координаты x, y, z и передаем по индексу 0
            glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)0);
            glEnableVertexAttribArray(Attrib_vertex); // Включаем массив атрибутов

            glVertexAttribPointer(Color_vertex, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));
            glEnableVertexAttribArray(Color_vertex);
        }
        if (count == 1) 
        {
            // Считываем координаты x, y, z и передаем по индексу 0
            glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)0);
            glEnableVertexAttribArray(Attrib_vertex); // Включаем массив атрибутов

            glVertexAttribPointer(Color_vertex, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));
            glEnableVertexAttribArray(Color_vertex);

            glVertexAttribPointer(Tex_Vertex, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(6 * sizeof(GLfloat)));
            glEnableVertexAttribArray(Tex_Vertex);
        }
        if (count == 2)
        {
            // Считываем координаты x, y, z и передаем по индексу 0
            glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)0);
            glEnableVertexAttribArray(Attrib_vertex); // Включаем массив атрибутов

            glVertexAttribPointer(Tex_Vertex, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(6 * sizeof(GLfloat)));
            glEnableVertexAttribArray(Tex_Vertex);
        }
        if (count == 3)
        {
            // Считываем координаты x, y, z и передаем по индексу 0
            glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (void*)0);
            glEnableVertexAttribArray(Attrib_vertex); // Включаем массив атрибутов

            glVertexAttribPointer(Color_vertex, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));
            glEnableVertexAttribArray(Color_vertex);
        }
        if (count == 4)
        {
            // Считываем координаты x, y, z и передаем по индексу 0
            glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 7 * sizeof(GLfloat), (void*)0);
            glEnableVertexAttribArray(Attrib_vertex); // Включаем массив атрибутов

            glVertexAttribPointer(Color_vertex, 4, GL_FLOAT, GL_FALSE, 7 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));
            glEnableVertexAttribArray(Color_vertex);
        }
                    
        
        glBindBuffer(GL_ARRAY_BUFFER, 0);
        if (count == 0 || count == 1) 
        {
            glBindTexture(GL_TEXTURE_2D, texture);

            //Куб
            glDrawArrays(GL_QUADS, 0, 24);
        }
        else if (count == 2)
        {
            glBindTexture(GL_TEXTURE_2D, texture);
            glBindTexture(GL_TEXTURE_2D, texture2);

            //Куб
            glDrawArrays(GL_QUADS, 0, 24);
        }
        else if(count == 3) 
        {
            //Тетраэдр
            glDrawArrays(GL_TRIANGLES, 0, 12);
        }
        else if (count == 4)
        {
            //шар
            glDrawArrays(GL_TRIANGLE_FAN, 0, 362);
        }

        glDisableVertexAttribArray(Attrib_vertex);
        glDisableVertexAttribArray(Color_vertex);
        //glBindVertexArray(0);
        // uniform - для любых двух шейдеров
        // attrib - для вершинного

        window.display();
    }

    glDeleteBuffers(1, &VBO);
    glUseProgram(0);
    glDeleteProgram(shaderProgram);
   
    return 0;
}