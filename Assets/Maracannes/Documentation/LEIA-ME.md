# ACERVO MARACANNES — 5 ANOS

Estado atual: dois ambientes independentes, com troca de cena nas portas. A montagem anterior foi preservada como arquivo histórico em Documentation/Arquivo_Anterior e não é carregada na visita.

## Abrir e visitar
Cena inicial: Assets/Teatro_Centro_SESI/Scenes/Teatro_Centro_SESI.unity.
Cena do acervo: Assets/Maracannes/Scenes/Maracannes_Acervo.unity.
As duas cenas estão habilitadas nos Build Settings; o teatro é a primeira. Menu Maracannes → Abrir visita integrada abre o teatro. A SampleScene foi preservada como recurso antigo, sem integrar a lista de build atual.

No saguão, à direita de quem entra, há uma porta com a placa ACERVO MARACANNES — 5 ANOS. E perto dela faz transição com fade, descarrega o teatro e abre o acervo. A porta SAÍDA — VOLTAR AO TEATRO faz o caminho inverso. O mesmo CharacterController, câmera de primeira pessoa e estado de ingresso são reutilizados. A porta não exige compra ou novo cadastro. As regras da bilheteria e da entrada do auditório original permanecem.

WASD e mouse: visita. Esc: pausa/liberação do cursor no acervo. Somente as portas têm interação no acervo. Não há janelas ou instruções E nos cartazes ou esculturas.

## Quatro salas
1. História: painéis estáticos baseados exclusivamente no texto fornecido, vitrines, memória, esculturas e móbile. Sem datas ou nomes históricos inventados. Reservas para registros históricos ainda não enviados.
2. Cinema: todos os 30 materiais da pasta da sala 2, incluindo os de outras edições conforme instrução expressa do usuário. Única legenda individual: título do filme, conferido nas imagens originais. Os dois cartazes de Escapism/Escapismo continuam distintos, com suas grafias próprias.
3. Brasil em Cena — 2025: 10 cartazes integrais, sem legendas externas e sem interação; instrumentos cenográficos, cerâmicas, fitas, móbile e esculturas abstratas em terracota/ocre/verde.
4. Memórias da Arte — 2026: 3 materiais integrais, sem legendas externas e sem interação; molduras volumétricas, esculturas metálicas, vitrine e móbile. Group 6 permanece panorâmico, inteiro e proporcional, agora ampliado fisicamente na parede.

A arquitetura expositiva é autoral, sem alegação de reproduzir uma ala real da Firjan. Os objetos abstratos são cenografia original, sem atribuição a artistas ou obras históricas.

## Recursos
Preservados os 43 originais, byte a byte, em Documentation/Originais. Inventário completo, hashes, ZIPs, dimensões, PDFs e folhas de contato em Documentation/Inventario. Images contém os derivados integrais já preparados anteriormente, sem redesenho ou mudança dos cartazes. Não houve remoção de material recebido.

Novas composições: 32 conjuntos apoiados em bases + 4 móbiles com sustentação, além das instalações e objetos existentes. Técnicas: órbitas de metal, lâminas em madeira/metal, constelações de hastes/esferas, arcos, equilíbrio, espirais espessas, vasos e discos. A circulação foi planejada em torno dessas bases.

Modelos externos:
- Vintage Video Camera, Poly Haven — modelo/fotografia Jenelle van Heerden, texturas James Ray Cock; CC0. https://polyhaven.com/a/vintage_video_camera
- Ceramic Vase 03, Poly Haven — James Ray Cock; CC0, aproximadamente 3 mil triângulos, FBX e textura 1k. https://polyhaven.com/a/ceramic_vase_03
Licença: https://polyhaven.com/license. Créditos adicionais junto aos modelos. Powered by Poly Haven: API consultada para downloads. Todos os recursos usados ficam locais; não há chamadas de rede na execução.

## Manutenção
MaracannesExhibit é somente metadado de inventário, sem Update nem interação. As imagens estáticas estão nos MeshRenderers filhos dos objetos identificados pelo ID S2/S3/S4. Sala 2 tem somente um TextMesh de título por cartaz; salas 3 e 4 não têm TextMesh filho do cartaz. Não associar MuseumPoint aos cartazes.

AcervoDoor: dados das duas portas. AcervoTravel: transição, spawn seguro e persistência do visitante. AcervoBootstrap: permite abrir a cena do acervo diretamente no Editor, usando o prefab do visitante existente somente se nenhum já estiver presente. AcervoRuntimeAudit é uma ferramenta de teste inativa sem seu arquivo de solicitação; não adiciona outro controlador.

As ferramentas de conversão e iluminação são opcionais. Conversão pode recriar a raiz Cenografia_Ampliada; não execute para editar somente uma posição ou uma legenda. Ajustes manuais normais nos objetos permanecem possíveis.

## Testes executados e evidências
acervo-runtime.txt: percurso real no Editor, bilheteria, entrada/saída com carga e descarga das cenas, visitante único, quatro salas, todos os cartazes, regras de legenda, nenhuma interação nas peças, retorno ao auditório e chegada ao palco original. Acervo_*.png em Evidencias: capturas reais da Unity. acervo-construcao.txt: relação dos materiais e composição da cena. Relatórios anteriores pertencem à montagem antiga e não descrevem as regras atuais.

Os testes de execução são do Editor; não foi gerado executável distribuído. As pendências documentais são vídeos e fotografias históricas independentes, que não constavam nas pastas fornecidas. A resolução dos arquivos pequenos permanece limitada pela fonte original.

Conferência adicional pela UI do Editor: acervo-portas-manual.txt registra entrada e saída com E e pausa/retomada com Esc. A janela Game deve receber foco para testar o teclado no Editor.
