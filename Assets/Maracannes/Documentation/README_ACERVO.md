Acervo atualizado: cinco salas. Ordem de visita:
01 História do Maracannes
02 Acervo audiovisual (todos os arquivos anteriores mantidos)
03 Maracannes Experience 2024 (ID interno 5)
04 Brasil em Cena 2025 (ID interno 3 preservado)
05 Memórias da Arte 2026 (ID interno 4 preservado)

Nove novos materiais de 2024: seis PDFs de uma página convertidos integralmente em PNG, e três JPEGs originais. Originais e hashes em experience2024-inventario.json; posições e materiais no catálogo posters.json. Os cartazes ficam estáticos, sem legendas externas ou interações.

Cenografia criada no projeto: estrelas sem nomes de celebridades, tapete vermelho, balizadores e cordões, ilha de projeção, claquete, rolos de película, prêmio abstrato, cadeiras de direção, estrutura de iluminação e refletores. Câmera Poly Haven CC0 já existente reutilizada. Sem novos downloads ou dependências.

Cena alterada: Assets/Maracannes/Scenes/Maracannes_Acervo.unity. Teatro e controlador preservados. Backup anterior em Documentation/Arquivo_Anterior/Acervo_Antes_Experience2024.unity. A antiga passagem direta entre audiovisual e Brasil em Cena foi fechada; dois portais no lado leste conectam a sala de 2024 entre elas. Três cartazes anteriores foram reposicionados para liberar os portais, sem remover conteúdo.

Qualidade dos arquivos 2026: as 14 imagens recebidas têm aproximadamente 160x227 pixels; os arquivos são preservados integralmente. Nenhuma restauração generativa aplicada.

Proteção contra quedas: as duas cenas possuem o grupo Colisoes_Perimetro_Antiqueda, com quatro barreiras externas invisíveis e uma laje inferior. Os limites são calculados a partir dos pisos existentes. MapSafetyBoundary adiciona VisitorFallGuard ao controlador atual, sem criar outro personagem. O guardião retorna ao último piso seguro ao cair abaixo de -0,5 m ou ultrapassar os limites, e reinicia a velocidade vertical. As áreas internas e os acessos permanecem livres.

Música ambiente: Audio/Ambiente_Lofi.mp3, arquivo fornecido pelo usuário e copiado sem alterar o original. Objeto Musica_Ambiente_Lofi nas cenas Teatro_Centro_SESI e Maracannes_Acervo. MuseumBackgroundMusic mantém uma única fonte entre as cenas: áudio 2D, loop, volume 0.035 (3,5%) e entrada suave. O volume pode ser ajustado no componente MuseumBackgroundMusic. A origem do arquivo foi indicada pelo usuário; não foi feita verificação independente da licença.
