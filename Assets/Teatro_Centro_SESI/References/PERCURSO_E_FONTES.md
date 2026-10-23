# Ampliação: fachada → hall → teatro

A cena integrada continua em `Scenes/Teatro_Centro_SESI.unity`, no mesmo projeto Metavision. A versão anterior do teatro foi preservada em `Scenes/Teatro/Teatro_Antes_Ampliacao.unity` como cópia de segurança, não carregada junto. As pastas Scenes/Fachada, Scenes/Hall e Scenes/Corredores estão reservadas para organização; não há cenas aditivas duplicando os ambientes.

## O que foi confirmado

Unidade: Teatro Firjan SESI Centro, Avenida Graça Aranha, 1, Rio de Janeiro. A foto IMG_8294 1 mostra a identificação na fachada e a programação de outubro de 2026. O painel público do local também informa esse endereço, a existência de café e mini galeria no foyer:
https://site.bileto.sympla.com.br/teatrofirjansesicentro/

Fachada pública compatível com a foto enviada:
https://www.timeout.com/pt/rio-de-janeiro/teatro/teatro-firjan-sesi-centro

Relato vinculado ao endereço, com café e mesas no interior:
https://www.minube.com.br/sitio-preferido/teatro-sesi-a3625775

As imagens pesquisadas de Casa Firjan (Botafogo), SESI São José dos Campos, Jacarepaguá, Campos e Macaé foram rejeitadas como referência visual deste hall. O PDF coletivo retornado nas buscas não comprova que a fotografia de hall seja do Centro.

## Referência fotográfica e estimativas

Fachada: montantes de pedra, vidraças com esquadrias e brises, marquise clara, programação laranja, letreiro TEATRO SESI, moldura do painel e entrada recuada baseados em IMG_8294 1. As texturas da programação, cartaz e reflexos de vidro são recortes dessa foto do usuário; não há rostos ou táxis nesses recortes. A projeção plana e a escala são aproximadas, sem levantamento de perspectiva ou cotas reais.

Calçada: padrão procedural em preto e branco inspirado na foto; não é uma reprodução pedra por pedra. O edifício representa o trecho inferior necessário ao acesso, não a torre inteira ou o quarteirão.

Hall, posição do café, mesas, galeria vazia, corredor, elevação e rampa: volumes provisórios, sem fotografias suficientes para afirmar fidelidade. Só a existência de café e mini galeria foi confirmada. Não se acrescentaram logotipos, obras ou balcão de recepção alegando que seriam reais. Os quadros vazios reservam espaço, sem inventar exposição.

O trajeto foi alinhado à circulação central existente da plateia para oferecer uma experiência contínua. Isso não comprova o traçado ou a posição das portas reais do prédio. A rampa é uma solução funcional da maquete, não um acesso real confirmado, nem um projeto de acessibilidade.

## Preservação e integração

O teatro, poltronas, palco, materiais e luzes anteriores foram reutilizados. A parede traseira inteiriça e as portas cenográficas antigas ficam desativadas; os originais permanecem no arquivo. Novos segmentos de parede deixam o vão central aberto. A nova raiz Percurso_Centro_SESI contém Fachada_Externa, Entrada_Principal, Hall_Entrada, Corredores_Acesso, Entrada_Teatro e Iluminacao_e_Cameras. A raiz Teatro_Centro_SESI mantém a organização original de arquitetura, plateia, balcão, equipamentos e iluminação.

## Materiais gratuitos

Granite Tile — Charlotte Baglioni / Poly Haven, CC0:
https://polyhaven.com/a/granite_tile
https://polyhaven.com/license
https://dl.polyhaven.org/file/ph-assets/Textures/jpg/1k/granite_tile/granite_tile_diff_1k.jpg
Powered by Poly Haven — https://polyhaven.com

Material importado adaptado em tonalidade e escala; não é uma identificação petrográfica da pedra do edifício. Madeira reutiliza o asset CC0 da primeira etapa. Portas e mobiliário foram modelados na Unity para evitar dependências e modelos incompatíveis com a foto. Foi consultado o modelo gratuito Stylized Door (Asset Store, Standard EULA), descartado pelo estilo inadequado; nenhum pacote pago ou login foi necessário. Licença ambientCG consultada em https://docs.ambientcg.com/license/; nenhum arquivo ambientCG importado.

## Exploração e ferramentas

Play inicia na calçada. WASD move a câmera com CharacterController, botão direito gira a visão. Botão `Percorrer entrada → palco` inicia a visita guiada; Esc interrompe. O percurso automático usa movimento com colisão e gravidade. Não havia controlador de jogador funcional no projeto antes desta ampliação; o novo componente é exclusivo da inspeção arquitetônica.

Menu Teatro SESI:
- Ampliar fachada e percurso integrado: atualiza apenas a raiz da ampliação e reabre a passagem do teatro; não recria o auditório.
- Renderizar novos ambientes: gera as vistas de fachada, hall, corredor e câmera de percurso em Lighting.
- Verificar percurso em Play: executa o trajeto com colisão e salva `percurso-runtime.txt`. O teste termina em modo de edição.

Reexecutar a ferramenta substitui edições manuais na raiz Percurso_Centro_SESI. O gerador antigo de teatro continua disponível e pode reconstruir a parede traseira; execute a ampliação depois dele para reabrir o acesso.

O controlador não oferece acesso ao balcão superior: ele integra o caminho principal até o palco. Não há NavMesh, nova aplicação, mudança de pipeline ou alteração dos sistemas de SampleScene. Os renders e testes não substituem a validação presencial do edifício.

## Resultado do teste

Teste executado em Play: PASS, 11/11 pontos, da calçada ao palco, usando CharacterController com colisão, gravidade e subida dos degraus. Console observado sem erros durante a execução. Relatório: percurso-runtime.txt. Cena retornou ao modo de edição. Renders da fachada, hall e corredor foram inspecionados e os textos invertidos corrigidos. As luminárias de circulação foram ajustadas para emitir para baixo.
