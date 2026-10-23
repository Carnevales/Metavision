# Teatro Firjan SESI Centro — implementação Unity

Cena: `Assets/Teatro_Centro_SESI/Scenes/Teatro_Centro_SESI.unity` no projeto Metavision, Unity 6000.5.8f1, URP 17.5.0. Cena carregada aditivamente para preservar SampleScene e sua câmera original. A câmera de inspeção do teatro tem prioridade de renderização enquanto essa cena está carregada.

## Referências e grau de certeza

- IMG_8292 1.png: referência principal do palco atual: frente reta mineral, escada central, ombreiras claras, fundo e pernas pretas, faixas vinho, PA suspenso, monitores, pedestais e piano à esquerda.
- IMG_8294 1.png: fachada e identificação SESI Centro. Usada para identificar a unidade, sem modelagem da fachada urbana.
- image 13 / image 15: referência da plateia vista do palco, balcão, cabine, painéis vinho e corrimão. Data/unidade dessas fotos não foi confirmada independentemente. Elementos de fundo são aproximações.
- image 24: palco com frente curva e duas escadas, incompatível com a foto recente. Não foi incorporada essa geometria ao palco Centro.
- image 17: cortina vermelha fechada, assentos vermelhos e corredores diferentes. Não foi misturada essa configuração à referência principal. Sem identificação independente da unidade.

Fonte dimensional oficial:
https://firjan.com.br/data/files/CB/36/40/97/97AE7910734FAA69D8284EA8/Rider_Teatro_Firjan_SESI_Centro_2025_shows.pdf

Rider armazenado nesta pasta. Boca 9,70 m x 4,48 m; profundidade 7,30 m; proscênio 2,32 m; pé direito 5,49 m. A profundidade foi interpretada como extensão total do piso modelado; conferir a convenção do rider antes de uso técnico. O pé direito/urdimento e a assimetria completa das coxias não foram reproduzidos como levantamento técnico.

Rider informa 246 lugares na plateia, 92 no balcão, 338 no total, com reservas para acessibilidade. O cenário tem 240 poltronas na plateia e 92 no balcão; não implementa mapa oficial nem acessibilidade completa. Largura de sala (15,6 m), elevação de palco (1,05 m), inclinação da plateia, teto e balcão são estimados. Não usar para planejamento de segurança ou produção técnica.

Pesquisa complementar:
https://www.timeout.com/pt/rio-de-janeiro/teatro/teatro-firjan-sesi-centro — fachada compatível com a foto do usuário.
https://www.firjan.com.br/noticias/reabertura-teatros-firjan-sesi-itaperuna-macae-e-centro-8AE4828C8ED4BE48018EE9857B0B5214-00.htm — informa reforma do Centro; imagens da galeria identificadas como Macaé foram descartadas.

## Assets e licença

Madeira: Fine Grained Wood, Poly Haven, CC0. Arquivo de cor 1k importado e usado no material URP Madeira_natural.
https://polyhaven.com/a/fine_grained_wood
https://polyhaven.com/license
https://dl.polyhaven.org/file/ph-assets/Textures/jpg/1k/fine_grained_wood/fine_grained_wood_col_1k.jpg
Metadados consultados pela API: Powered by Poly Haven (https://polyhaven.com).

Modelos próprios produzidos em Unity: poltrona com encosto arredondado, cortinas com pregas por mesh, piano aproximado, monitores, PA modular, pedestais e varas de luz. Não foram importados modelos de terceiros genéricos que alterassem a identidade do espaço. Fotos fornecidas são referências, não texturas de materiais.

## Ajustes

Menu `Teatro SESI > Criar ou abrir cena do teatro`: gera ou atualiza a cena dedicada dentro do projeto. Selecione a raiz Teatro_Centro_SESI para ajustar as dimensões no componente TeatroDimensions. Use `Atualizar teatro selecionado` e salve a cena. A reconstrução substitui somente os filhos dessa raiz e o prefab de poltrona; edições manuais nesses filhos serão substituídas. Materiais são reutilizados, mas suas cores são reaplicadas pelo gerador.

Três câmeras: Frente (ativa), Palco e Balcão (desativadas). Para outro ângulo, habilite a câmera desejada e desabilite Frente no Inspector. Menu `Renderizar vistas de verificacao` salva imagens em Lighting.

## Limites do resultado

Representação aproximada com medidas principais verificadas, não réplica fotogramétrica. Cabine, portas e balcão simplificados; piano e equipamentos representam formas, não modelos comerciais exatos. Escada de manutenção, cabos, extintores, sinalização, rotas laterais, coxias completas e acabamento fino ainda não reproduzidos integralmente. Iluminação é uma aproximação visual em tempo real, com preenchimento artificial para leitura da sala; não há bake de iluminação. O cenário não inclui controlador de navegação ou novo sistema de gameplay.

Verificação executada pelo gerador: materiais/shaders, persistência da cena e três renderizações por câmeras Unity. Ver `validation.txt` e imagens em Lighting. A compilação e o teste Play são conferidos no Editor durante a implementação.
